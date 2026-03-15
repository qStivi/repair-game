using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Used for creating Context of a Condition.  
/// </summary>
public interface IConditionContext
{
    string SourceId { get; }
    IModificationSource Source { get; }
    int TargetId { get; }
    ModifierDomain ModificationDomain { get; }

}

