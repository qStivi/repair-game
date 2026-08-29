using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

public sealed class ModifierSystem
{
    private readonly Dictionary<int, IModifierTarget> _targets = new();
    private readonly Dictionary<string, IModificationSource> _sources = new();
    private readonly Dictionary<StackKey, ModifierApplication> _appsByKey = new();
    private readonly Dictionary<StatBucketKey, List<ModifierApplication>> _appsByStat = new();
    // Versioning: Whenever a modifier is added or removed, the version of the affected stat bucket is incremented. 
    // This allows caching of computed stat values and invalidating them when the underlying modifiers change.
    private readonly Dictionary<StatBucketKey, long> _bucketVersions = new();

    private readonly List<ModifierApplication> _apps = new();

    public void RegisterSource(IModificationSource source) => _sources[source.SourceId] = source;
    public void UnregisterSource(IModificationSource source) => _sources.Remove(source.SourceId);
    public void RegisterTarget(IModifierTarget target) => _targets[target.TargetId] = target;
    public void UnregisterTarget(IModifierTarget target) => _targets.Remove(target.TargetId);

    public ModifierApplication Apply(ModifierPrototypeSO proto, IModificationSource source, IModifierTarget targetOrNull, float now)
    {
        int? targetId = targetOrNull?.TargetId;

        if (targetOrNull != null && !proto.CanApply(targetOrNull, source))
            return null;

        RegisterTarget(targetOrNull);
        RegisterSource(source);

        float? expireAt = ComputeExpireAt(proto.Duration, now);

        var key = MakeStackKey(proto, source, targetId);

        if (_appsByKey.TryGetValue(key, out var existing))
        {
            // stacking behavior
            switch (proto.StackMode)
            {
                case StackMode.None:
                    return existing;

                case StackMode.AddStacks:
                    existing.AddStacks(1);
                    break;

                case StackMode.RefreshDuration:
                    Refresh(existing, proto.Duration, now);
                    break;

                case StackMode.AddAndRefresh:
                    existing.AddStacks(1);
                    Refresh(existing, proto.Duration, now);
                    break;
            }

            return existing;
        }

        var app = proto.CreateApplication(targetId, source.SourceId, key, now, expireAt);
        _appsByKey[key] = app;
        _apps.Add(app);

        var statBucketKey = new StatBucketKey(targetOrNull.Domain, proto.GetStatAsInt(), targetId ?? 0);

        if (!_appsByStat.TryGetValue(statBucketKey, out var modifierApplications))
        {
            modifierApplications = new List<ModifierApplication>();
            _appsByStat[statBucketKey] = modifierApplications;
        }

        modifierApplications.Add(app);
        return app;
    }



    //Rückwärts: Das Entfernen verschiebt nur Elemente mit höherem Index nach links.
    //Aber du bist gerade bei einem hohen Index und gehst nach unten → du “verlierst” nichts, du überspringst nichts.
    public void RemoveBySource(string sourceId)
    {
        for (int i = _apps.Count - 1; i >= 0; i--)
        {
            if (_apps[i].SourceId == sourceId)
                RemoveAt(i);
        }
    }

    public void RemoveApplication(ModifierApplication app)
    {
        int idx = _apps.IndexOf(app);
        if (idx >= 0) RemoveAt(idx);
    }

    public void Tick(float now)
    {
        for (int i = _apps.Count - 1; i >= 0; i--)
            if (_apps[i].IsExpired(now))
                RemoveAt(i);
    }

    private void RemoveAt(int index)
    {
        var app = _apps[index];
        _apps.RemoveAt(index);
        _appsByKey.Remove(app.Key);
    }

    public long GetBucketVersion(StatBucketKey key)
    {
        return (!_bucketVersions.TryGetValue(key, out var version)) ? version : 0;
    }

    public long GetCombindedVersion(IModifierTarget target, int statId)
    {
        var targetedKey = new StatBucketKey(target.Domain, statId, target.TargetId);
        var globalKey = new StatBucketKey(target.Domain, statId);

        long targetedVersion = GetBucketVersion(targetedKey);
        long globalVersion = GetBucketVersion(globalKey);

        return HashCode.Combine(targetedVersion, globalVersion);
    }

    public IEnumerable<ModifierApplication<T>> Query<T>(
    IModifierTarget target,
    int statId,
    float now)
    where T : IStatOperable<T>
    {
        // Target-specific modifiers
        var targetedKey = new StatBucketKey(
            target.Domain,
            statId,
            target.TargetId);

        if (_appsByStat.TryGetValue(targetedKey, out var targeted))
        {
            foreach (var app in targeted)
            {
                if (app.IsExpired(now))
                    continue;


                if (app is not ModifierApplication<T> typedApp)
                {
                    throw new InvalidOperationException(
                        $"Stat bucket {target.Domain}/{statId} " +
                        $"contains {app.GetType().Name}, " +
                        $"but {typeof(T).Name} was requested.");
                }

                if (!_sources.TryGetValue(
                        typedApp.SourceId,
                        out var source))
                {
                    throw new KeyNotFoundException(
                        $"No source found for SourceId " +
                        $"'{typedApp.SourceId}'.");
                }

                if (!typedApp.Prototype.CanApply(target, source))
                    continue;

                yield return typedApp;
            }
        }

        // Global modifiers
        var globalKey = new StatBucketKey(
            target.Domain,
            statId,
            0);

        if (_appsByStat.TryGetValue(globalKey, out var global))
        {
            foreach (var app in global)
            {
                if (app.IsExpired(now))
                    continue;

                if (app is not ModifierApplication<T> typedApp)
                {
                    throw new InvalidOperationException(
                        $"Stat bucket {target.Domain}/{statId} " +
                        $"contains {app.GetType().Name}, " +
                        $"but {typeof(T).Name} was requested.");
                }
                if (!_sources.TryGetValue(
                        typedApp.SourceId,
                        out var source))
                {
                    throw new KeyNotFoundException(
                        $"No source found for SourceId " +
                        $"'{typedApp.SourceId}'.");
                }

                if (!typedApp.Prototype.CanApply(target, source))
                    continue;

                yield return typedApp;
            }
        }
    }
    private static StackKey MakeStackKey(ModifierPrototypeSO proto, IModificationSource source, int? targetId)
    {
        return new StackKey(proto.UniqueId, source.SourceId, targetId);
    }

    private static float? ComputeExpireAt(DurationSpec spec, float now)
    {
        return spec.Type == DurationType.Timed ? now + spec.Seconds : (float?)null;
    }

    private static void Refresh(ModifierApplication app, DurationSpec spec, float now)
    {
        if (spec.Type == DurationType.Timed)
            app.RefreshTimed(now, spec.Seconds);
    }
}