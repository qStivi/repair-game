using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [field: SerializeField] public long UnlockCost { get; private set; }
    [field: SerializeField] public long ActivationCost { get; private set; }
    [field: SerializeField] public Rarity Rarity { get; private set; }
    [field: SerializeField] public long Level { get; private set; }
    [SerializeField] private CardBehavior cardBehavior;

    private void Awake()
    {
        if (cardBehavior != null) cardBehavior.SetCard(this);
    }

    public bool IsUnlockable()
    {
        return cardBehavior != null && cardBehavior.IsUnlockable();
    }

    public bool IsUpgradeable()
    {
        return cardBehavior != null && cardBehavior.IsUpgradeable();
    }
}