using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Used for creating Context of a Condition.  
/// </summary>
public interface IConditionContext
{
    GameObject Target { get; }
}

//Gerade ist der Kontext beispielsweise einfach das Gebäude später kann es aber komplexer werden.
public sealed class BuildingContext : IConditionContext 
{
    public GameObject Target => Building.gameObject;
    public AbstractBuilding Building { get; }
    public BuildingContext(AbstractBuilding building) => Building = building;

}
