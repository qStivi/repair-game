public abstract class Wall : AbstractDefenseBuilding
{
    protected int defenseToolSlots;
    protected int repairAmount;
    protected bool repairInProgress;

    protected override void InitiateTimers()
    {
        repairIntervallTimer = new TimerManager(repairIntervall.GetValue());
    }

    protected void StartRepair()
    {
        repairInProgress = true;
        TimerManager repairPerSecond;
    }
}