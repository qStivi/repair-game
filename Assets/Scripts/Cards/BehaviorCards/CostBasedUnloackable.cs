using UnityEngine;

[CreateAssetMenu(fileName = "CostBasedUnlockable",
    menuName = "Card Behaviors/Cost Based Unlockable")]
public class CostBasedUnlockable : CardBehavior
{
    public Cost requiredCost;

    public override bool IsUnlockable()
    {
        // Custom unlock logic based on cost
        return ((IngredientCard)Card).Costs >= requiredCost;
    }

    public override bool IsUpgradeable()
    {
        // Custom upgrade logic
        return ((IngredientCard)Card).Costs / 2 >= requiredCost;
    }
}