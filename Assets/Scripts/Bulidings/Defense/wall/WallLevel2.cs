using System;

public class WallLevel2 : AbstractWall, IUpgradable
{
    protected override void Start()
    {
        base.Start();
        currentLevel = 2;
        hitpoints = 600;
        repairIntervall = 10.0;
        repairCost.Stone = 1;
        repairAmount = 100;
        rangeBoost = 1;
        unitSlots = 4;
        defenseToolSlots = 1;
        UpgradeCosts = new Cost(45, 8, 3);
    }

    public Cost UpgradeCosts { get; private set; }

    public void Upgrade()
    {
        throw new NotImplementedException();
    }
}