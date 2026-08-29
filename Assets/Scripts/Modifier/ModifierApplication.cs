

public abstract class ModifierApplication
{
    public ModifierPrototypeSO Prototype { get; }
    public int? TargetId { get; }      // null => global modifier (affects all matching targets)
    public string SourceId { get; }
    public StackKey Key { get; }

    public int Stacks { get; private set; } = 1;

    // duration state
    public float StartTime { get; private set; }
    public float? ExpireAtTime { get; private set; } 

    public ModifierApplication(ModifierPrototypeSO proto, int? targetId, string sourceId, StackKey key, float now, float? expireAt)
    {
        Prototype = proto;
        TargetId = targetId;
        SourceId = sourceId;
        Key = key;
        StartTime = now;
        ExpireAtTime = expireAt;
    }

    public bool IsExpired(float now) => ExpireAtTime.HasValue && now >= ExpireAtTime.Value;

    public void AddStacks(int amount) => Stacks += amount;

    public void RefreshTimed(float now, float durationSeconds)
    {
        var newExpire = now + durationSeconds;
        ExpireAtTime = ExpireAtTime.HasValue ? System.Math.Max(ExpireAtTime.Value, newExpire) : newExpire;
    }
}

public sealed class ModifierApplication<T>
    : ModifierApplication
    where T : IStatOperable<T>
{
    public ModifierPrototypeSO<T> TypedPrototype { get; }

    public T Value => TypedPrototype.Value;

    public ModifierApplication(
        ModifierPrototypeSO<T> proto,
        int? targetId,
        string sourceId,
        StackKey key,
        float now,
        float? expireAt)
        : base(
            proto,
            targetId,
            sourceId,
            key,
            now,
            expireAt)
    {
        TypedPrototype = proto;
    }
}
