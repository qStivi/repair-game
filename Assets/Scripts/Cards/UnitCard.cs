using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public long ActionSpeed { get; private set; }
    [field: SerializeField] public long ActionPoints { get; private set; }
    [field: SerializeField] public long TargetCount { get; private set; }
    [field: SerializeField] public List<EquipmentCard> IsGoodWith { get; private set; }
    [field: SerializeField] public List<EquipmentCard> IsBadWith { get; private set; }
    [field: SerializeField] public EquipmentCard Equipment { get; private set; }
    [field: SerializeField] public Cost Costs { get; private set; }
    [field: SerializeField] public List<Enemy> StrongAgainst { get; private set; }
    [field: SerializeField] public List<Enemy> WeakAgainst { get; private set; }

    public bool IsStrongAgainst(Enemy enemy)
    {
        return StrongAgainst.Contains(enemy);
    }

    public bool IsWeakAgainst(Enemy enemy)
    {
        return WeakAgainst.Contains(enemy);
    }
}