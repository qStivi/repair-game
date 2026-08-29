using System;
using UnityEngine;

public abstract class BuildingModifierPrototypSO<T> : ModifierPrototypeSO<T> where T : IStatOperable<T>
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
    public override int GetStatAsInt() => Convert.ToInt32(StatType);
}


[CreateAssetMenu(menuName = "Modifiers/BuildingFloatPrototype")]
public sealed class BuildingFloatModifierBlueprintSO : BuildingModifierPrototypSO<FloatValue>
{

    public override string GetEffectSignature()
    {
        float value = Mathf.Round(Value * 10000f) / 10000f;
        string baseString = base.GetEffectSignature();
        return $"{baseString}|value={value}";
    }
}
[CreateAssetMenu(menuName = "Modifiers/BuildingCostPrototype")]
public sealed class BuildingCostModifierBlueprintSO : BuildingModifierPrototypSO<Cost>
{
    public override string GetEffectSignature()
    {
        string baseString = base.GetEffectSignature();
        return $"{baseString}|value={Value}";
    }
}







