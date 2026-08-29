using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Unit Prototype")]
public abstract class UnitModifierPrototypSO<T> : ModifierPrototypeSO<T> where T : IStatOperable<T>
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