using UnityEngine;

public abstract class ModifierBaseConditionSO : ScriptableObject, ICondition
{
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
