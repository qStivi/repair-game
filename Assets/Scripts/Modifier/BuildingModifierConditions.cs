using UnityEngine;

public abstract class BuildingModifierConditionSO : ScriptableObject, ICondition
{
    public abstract bool IsMet(BuildingContext ctx);

    //ADAPTER: it makes the class usable as ICondition everywhere.
    bool ICondition.IsMet(IConditionContext context)
    {
        // Try to “adapt” the generic context to the specific one
        if (context is BuildingContext bctx)
            return IsMet(bctx);

        return false;
    }
}

[CreateAssetMenu(menuName = "Modifications/Conditions/Building/MinLevel")]
public sealed class MinLevel : BuildingModifierConditionSO
{
    [SerializeField] private int minLevel;

    public override bool IsMet(BuildingContext ctx) => ctx.Building.currentLevel >= minLevel;
}

[CreateAssetMenu(menuName = "Modifications/Conditions/Building/ExactLevel")]
public sealed class ExactLevel : BuildingModifierConditionSO
{
    [SerializeField] private int exactLevel;

    public override bool IsMet(BuildingContext ctx) => ctx.Building.currentLevel == exactLevel;
}




