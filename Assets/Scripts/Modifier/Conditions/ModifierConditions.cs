using System;
using UnityEngine;

public abstract class ModifierBaseConditionSO : ScriptableObject, ICondition
{
    [SerializeField, HideInInspector] private string uniqueID;
    public string UniqueId => uniqueID;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(UniqueId))
            uniqueID = Guid.NewGuid().ToString("N");
    }
#endif
    public abstract bool IsMet(IConditionContext context);
 
}

[CreateAssetMenu(menuName = "Modification/Conditions/AND")]
public class AndCondition : ModifierBaseConditionSO
{
    [SerializeField] private ICondition[] children;
    public override bool IsMet(IConditionContext ctx)
    {
        foreach (var c in children)
        {
            if (c != null && !c.IsMet(ctx))
                return false;
        }
        return true;
    }
}

[CreateAssetMenu(menuName = "Modification/Conditions/OR")]
public class OrCondition : ModifierBaseConditionSO
{
    [SerializeField] private ICondition[] children;
    public override bool IsMet(IConditionContext ctx)
    {
        foreach (var c in children)
        {
            if (c != null && c.IsMet(ctx))
                return true;
        }
        return false;
    }
}

[CreateAssetMenu(menuName = "Modification/Conditions/NOT")]
public class NotCondition : ModifierBaseConditionSO
{
    [SerializeField] private ICondition child;
    public override bool IsMet(IConditionContext ctx) => child == null || child.IsMet(ctx);
}
