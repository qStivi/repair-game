using System.Collections.Generic;
using System;
using UnityEngine;
using JetBrains.Annotations;

#region Enums
public enum StatTypeBuilding
{
    Hitpoints = 1,
    BuildDuration = 2,
    BuildingCosts = 3,
    RepairIntervall = 4,
    RepairCost = 5,
    RepairDuration = 6,
    ProductionIntervall = 7,
    ProductionAmount = 8,
    RangeBoost = 9,
    UnitSlots = 10,
}

//Performante Mehrfachauswahl mittels 0001 | 0100 (= Production | Storage) => speichert "0101" möglich; max. 32bit bei Int; None als kein Flag gesetzt wichtig
[Flags]
public enum BuildingCategory
{
    None = 0,
    Production = 1 << 0,
    Defense = 1 << 1,
    Storage = 1 << 2
}
#endregion


#region IStatEntry: um die unterschiedlichen Typen von Value zu berücksichten und im Inspektor auswählen zu können
public interface IStatEntry<TStatType> where TStatType: Enum
{
   TStatType StatType { get; }
   int StatId => Convert.ToInt32(StatType);
   IStat CreateStat(IModifierTarget owner);
}

[Serializable]
public class FloatStatEntry<TStatType> : IStatEntry<TStatType> where TStatType: Enum
{
    public float value;
    public TStatType StatType { get; }

    public IStat CreateStat(IModifierTarget owner) => new Stat<FloatValue>(value, owner, StatId);

   
}
[Serializable]
public class CostStatEntry<TStatType> : IStatEntry<TStatType> where TStatType: Enum
{
    public Cost value; 
    public TStatType StatType { get; }

    public IStat CreateStat(IModifierTarget owner) => new Stat<Cost>(value, owner, StatId);

}
#endregion


//Nullable, da beim letzten Level keine Updatekosten vorhanden sind => Idee: Updateable darüber abfragbar
[Serializable]
public class BuildingLevelData
{
    public int level;

    [SerializeReference] // bedeutet dass Unity bei diesem Feld Instanzen verwalteter Klassen (also „normale“ C#-Referenztypen) per Referenz und polymorph serialisieren soll. 
    public List<IStatEntry<StatTypeBuilding>> stats;

    #nullable enable
    public Cost? levelUpCost;
    #nullable disable
}


[CreateAssetMenu(fileName = "BuildingDefiniton", menuName = "Buildings/Definition")]
public class BuildingDefinition : ScriptableObject
{
    public string id;
    public BuildingCategory category;
    public List<BuildingLevelData> levels; 
}




/*public class BuildingLevelData : ScriptableObject
{
    [Header("General Values of Buildings")]
    public int hitpoints;

    [Tooltip("Wie lange muss der Spieler den Bildschirm berühren bis der Bauprozess abgeschlossen ist. Meistens erfolgt dies direkt.")]
    public float buildDuration;

    [Tooltip("Das Intervall in dem das Gebäude spätestens repariert werden muss, sonst wird es zerstört.")]
    public double repairIntervall;

    public Cost buildingCosts;
    public int level;
    public Cost repairCost;
    public float repairDuration;
}*/
