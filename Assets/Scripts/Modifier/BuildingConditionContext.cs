using UnityEngine;
/// <summary>
/// BuildingContextInterface, wenn es später ggf. noch andere Building auf der Main Map (Meta Progression) gibt.
/// </summary>
public interface IBuildingConditionContext : IConditionContext
{
    public AbstractBuilding Building { get; }
}

public sealed class BuildingContext : IBuildingConditionContext
{
    public AbstractBuilding Building { get; }

    public string SourceId => Source.SourceId;

    public IModificationSource Source { get; }

    public int TargetId => Building.TargetId;

    public ModifierDomain ModificationDomain => ModifierDomain.Building;

    public BuildingContext(AbstractBuilding building, IModificationSource source)
    {
        Building = building;
        Source = source;
    }

}
