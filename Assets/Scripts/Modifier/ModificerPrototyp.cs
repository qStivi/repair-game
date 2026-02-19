using UnityEngine;

public enum ModifierDomain
{
    Building, 
    Unit
}

public abstract class ModifierPrototypeSO : ScriptableObject
{
    public virtual ModifierDomain Domain { get; }
    public float Value;
    public DurationSpec Duration;
    public ModifierBaseConditionSO[] baseModifierConditions;

}

[CreateAssetMenu(menuName = "Modifiers/Building Blueprint")]
public class BuildingModifierBlueprintSO : ModifierPrototypeSO
{
    public override ModifierDomain Domain => ModifierDomain.Building;

    public StatTypeBuilding StatType { get; set; }

    public BuildingModifierConditionSO[] buldingModifierConditions;
  
}

[CreateAssetMenu(menuName = "Modifiers/Unit Blueprint")]
public class UnitModifierBlueprintSO : ModifierPrototypeSO
{
    public override ModifierDomain Domain => ModifierDomain.Unit;

    //UNIT STAT 
    //UNIT CONDITIONS


}


