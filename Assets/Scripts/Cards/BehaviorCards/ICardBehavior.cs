public interface ICardBehavior
{
    void SetCard(Card card);
    bool IsUnlockable();
    bool IsUpgradeable();
}