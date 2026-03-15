using UnityEngine;

[CreateAssetMenu(menuName = "Modifications/Conditions/Building/MinLevel")]
public sealed class MinLevel : BuildingModifierConditionSO
{
    [SerializeField] private int minLevel;

    public override bool IsMet(IBuildingConditionContext ctx) => ctx.Building.currentLevel >= minLevel;
}
[CreateAssetMenu(menuName = "Modifications/Conditions/Building/MaxLevel")]
public sealed class MaxLevel : BuildingModifierConditionSO
{
    [SerializeField] private int maxLevel;

    public override bool IsMet(IBuildingConditionContext ctx) => ctx.Building.currentLevel <= maxLevel;
}

[CreateAssetMenu(menuName = "Modifications/Conditions/Building/ExactLevel")]
public sealed class ExactLevel : BuildingModifierConditionSO
{
    [SerializeField] private int exactLevel;

    public override bool IsMet(IBuildingConditionContext ctx) => ctx.Building.currentLevel == exactLevel;
}


