using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public enum ModifierDomain
{
    Building, 
    Unit
}

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

public class BuildingModifierBlueprintSO : ModifierPrototypeSO
{
    public override ModifierDomain Domain => ModifierDomain.Building;

    public StatTypeBuilding StatType;

    public BuildingModifierConditionSO[] buildingConditionModifier;

    public override string GetEffectSignature()
    {
       
        string baseString = base.GetEffectSignature();
        return $"base={baseString}|statType={StatType}|buildConds={CondListSig(buildingConditionModifier)}";
    }

    protected override IConditionContext CreateContext(IModifierTarget target, IModificationSource source)
    {
        if (target is AbstractBuilding building)
            return new BuildingContext(building, source);

        throw new ArgumentException(
            $"{name}: Expected target of type AbstractBuilding but got {target?.GetType().Name ?? "null"}"
        );
      
    }

    protected override bool CanApplyToDomainSpecifc(IConditionContext ctx)
    {
        if(buildingConditionModifier == null) return true;

        if(ctx is not IBuildingConditionContext bctx)
        {
            throw new ArgumentException(
                    $"{name}: Expected target of type AbstractBuilding but got {ctx?.GetType().Name ?? "null"}"
                );
        }
       
        for (int i = 0; i < buildingConditionModifier.Length; i++)
        {
            var cond = buildingConditionModifier[i];
            if (cond != null && !cond.IsMet(bctx))
                return false;

        }
        return true;

    }
    public override int GetStatAsInt() => (int)StatType;
}
[CreateAssetMenu(menuName = "Modifiers/BuildingFloatPrototype")]
public sealed class BuildingFloatModifierBlueprintSO : BuildingModifierBlueprintSO
{
 
    public FloatValue Value;
    public override string GetEffectSignature()
    {
        float value = Mathf.Round(Value * 10000f) / 10000f;
        string baseString = base.GetEffectSignature();
        return $"{baseString}|value={value}";
    }
}
[CreateAssetMenu(menuName = "Modifiers/BuildingCostPrototype")]
public sealed class BuildingCostModifierBlueprintSO : BuildingModifierBlueprintSO
{

    public Cost Value;
    public override string GetEffectSignature()
    {
        string baseString = base.GetEffectSignature();
        return $"{baseString}|value={Value.ToString()}";
    }
}




[CreateAssetMenu(menuName = "Modifiers/Unit Prototype")]
public class UnitModifierBlueprintSO : ModifierPrototypeSO
{
    public override ModifierDomain Domain => ModifierDomain.Unit;

    public override string GetEffectSignature()
    {
        throw new System.NotImplementedException();
    }

    protected override IConditionContext CreateContext(IModifierTarget target, IModificationSource source)
    {
        throw new NotImplementedException();
    }

    public override int GetStatAsInt()
    {
        throw new NotImplementedException();
    }

    //UNIT STAT 
    //UNIT CONDITIONS


}


