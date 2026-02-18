internal interface IUpgradable
{
    Cost UpgradeCosts { get; }
    void Upgrade();
}