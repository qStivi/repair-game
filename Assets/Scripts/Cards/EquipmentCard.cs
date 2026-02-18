using System.Collections.Generic;
using UnityEngine;

public class EquipmentCard : Card
{
    [field: SerializeField] public long Size { get; private set; }
    [field: SerializeField] public float UnitCostMod { get; private set; }
    [field: SerializeField] public float SizeMod { get; private set; }
    [field: SerializeField] public float ActionSpeedMod { get; private set; }
    [field: SerializeField] public float ActionPointsMod { get; private set; }
    [field: SerializeField] public float TargetCountMod { get; private set; }
    [field: SerializeField] public List<DmgType> DmgType { get; private set; }
    public Dictionary<UnitCard, long> CanBeUsedBy { get; private set; }
}