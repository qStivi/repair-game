using System;
using System.Linq;
using UnityEngine;

public abstract class ModifierPrototypeSO : ScriptableObject
{
    [SerializeField, HideInInspector] private string uniqueId; 
    public string UniqueId => uniqueId;
    public virtual ModifierDomain Domain { get; }
    public ModificationType ModificationType;
    public DurationSpec Duration;
    public StackMode StackMode;
    public ModifierBaseConditionSO[] baseModifierConditions;

    public bool CanApply(IModifierTarget target, IModificationSource source)
    {
        if(Domain != target.Domain) return false;

        IConditionContext ctx = null;

        if(baseModifierConditions != null)
        {
            ctx = CreateContext(target, source);
            for (var i = 0; i < baseModifierConditions.Length; i++)
            {
                var baseCondition = baseModifierConditions[i];
                if (baseCondition != null && !baseCondition.IsMet(ctx))
                    return false;
            }
        }

        return CanApplyToDomainSpecifc(ctx ?? CreateContext(target, source));

    }
    protected virtual bool CanApplyToDomainSpecifc(IConditionContext ctx)
    {
        return true;
    }

    protected abstract IConditionContext CreateContext(IModifierTarget target, IModificationSource source);

    public abstract int GetStatAsInt();
    public abstract ModifierApplication CreateApplication(
        int? targetId,
        string sourceId,
        StackKey key,
        float now,
        float? expireAt);
    

    public virtual string GetEffectSignature()
    {

        string durationSigniature = Duration.ToSignatureString();

        return $"domain={Domain}|mod={ModificationType}|dur={durationSigniature}|baseCond={CondListSig(baseModifierConditions)}|";

    }
    protected static string CondListSig(ICondition[] conds)
    {
        if (conds == null || conds.Length == 0) return "";
        return string.Join(",", conds.Where(c => c != null).Select(c => c.UniqueId));
    }

    // Bei Erstellung ID zuweisen und Checken, ob bereits vorhanden.
#if UNITY_EDITOR
    //Ggf. Später nur bei spezielleren Calls und nicht jeder Validation (erst relevant, wenn viele Prototyps vorhanden)
    protected virtual void OnValidate()
    {
        if(string.IsNullOrEmpty(uniqueId))
            uniqueId = Guid.NewGuid().ToString("N");

        ModifierPrototypeDuplicateValidator.Validate(this);
    }
#endif

}

public abstract class ModifierPrototypeSO<T>
    : ModifierPrototypeSO
    where T : IStatOperable<T>
{
    [SerializeField]
    private T value;

    public T Value => value;

    public override ModifierApplication CreateApplication(
        int? targetId,
        string sourceId,
        StackKey key,
        float now,
        float? expireAt)
    {
        return new ModifierApplication<T>(
            this,
            targetId,
            sourceId,
            key,
            now,
            expireAt);
    }
}
