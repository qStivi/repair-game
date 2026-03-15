using UnityEngine;

/// <summary>
/// Used as BaseType for all different Types of Conditons
/// </summary>
public interface ICondition : IIdentifiable<string>
{
    public abstract bool IsMet(IConditionContext context);
}



