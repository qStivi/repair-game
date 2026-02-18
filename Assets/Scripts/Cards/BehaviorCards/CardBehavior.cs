using UnityEngine;

public abstract class CardBehavior : ScriptableObject, ICardBehavior
{
    protected Card Card;

    public void SetCard(Card card)
    {
        Card = card;
    }

    public abstract bool IsUnlockable();
    public abstract bool IsUpgradeable();
}