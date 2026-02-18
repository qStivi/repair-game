using System.Collections.Generic;
using UnityEngine;

public class EffectCard : Card
{
    [field: SerializeField] public long Yield { get; private set; }
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public Cost Costs { get; private set; }
    public Dictionary<IngredientCard, long> IngredientsNeeded { get; private set; }

    // public abstract void ApplyEffect();
    // TODO Implement this similar to BehaviorCards
}