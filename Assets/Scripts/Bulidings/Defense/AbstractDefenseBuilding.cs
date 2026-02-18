public class AbstractDefenseBuilding : AbstractBuilding
{
    protected Stat<FloatValue> rangeBoost;
    protected Stat<FloatValue> unitSlots;

    protected override void LoadStats()
    {
        base.LoadStats();
        rangeBoost = (Stat<FloatValue>)stats[StatTypeBuilding.RangeBoost];
        unitSlots = (Stat<FloatValue>)stats[StatTypeBuilding.UnitSlots];
    }
}