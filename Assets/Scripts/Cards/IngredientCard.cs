using UnityEngine;

public class IngredientCard : Card
{
    [field: SerializeField] public GroundType PreferredGroundType { get; private set; }
    [field: SerializeField] public float Yield { get; private set; }
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public Cost Costs { get; private set; }

    public float GetYield(GroundType groundType)
    {
        if (groundType == PreferredGroundType) return Yield;
        return Yield / 2;
    }
}