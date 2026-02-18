using UnityEngine;

[CreateAssetMenu(fileName = "LevelBasedUnlockable",
    menuName = "Card Behaviors/Level Based Unlockable")]
public class LevelBasedUnlockable : CardBehavior
{
    public int requiredLevel;

    public override bool IsUnlockable()
    {
        // Custom unlock logic based on level
        return Card.Level >= requiredLevel;
    }

    public override bool IsUpgradeable()
    {
        // Custom upgrade logic
        return Card.Level / 2 >= requiredLevel;
    }
}