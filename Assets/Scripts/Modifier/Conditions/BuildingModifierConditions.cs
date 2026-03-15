using System;
using UnityEngine;

public abstract class BuildingModifierConditionSO : ScriptableObject, ICondition
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

    public abstract bool IsMet(IBuildingConditionContext ctx);

    //ADAPTER: it makes the class usable as ICondition everywhere.
    bool ICondition.IsMet(IConditionContext context)
    {
        // Try to “adapt” the generic context to the specific one
        if (context is IBuildingConditionContext bctx)
            return IsMet(bctx);

        return false;
    }
}




