using UnityEngine;

/// <summary>
/// Used as BaseType for all different Types of Conditons
/// </summary>
public interface ICondition
{
    public abstract bool IsMet(IConditionContext context);
}



