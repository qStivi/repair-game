using System;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public enum RessourceType
{
    Coins,
    Wood,
    Stone,
    PotionTools
}

[SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
[Serializable]
public class Cost : IComparable<Cost>, IStatOperable<Cost>
{
    [field: SerializeField] public float Coins { get; set; }

    [field: SerializeField] public float Wood { get; set; }

    [field: SerializeField] public float Stone { get; set; }

    [field: SerializeField] public float PotionTools { get; set; }

    public Cost(float coins = 0, float wood = 0, float stone = 0, float potionTools = 0)
    {
        Coins = coins;
        Wood = wood;
        Stone = stone;
        PotionTools = potionTools;
    }

    public string ToStringIfNotZeroOrNull()
    {
        var stringToPrint = string.Empty;
        if (Coins != 0) stringToPrint += $"{Coins} Coins";
        if (Wood != 0) stringToPrint += $"{Wood} Wood";
        if (Stone != 0) stringToPrint += $"{Stone} Stone";
        if (PotionTools != 0) stringToPrint += $"{PotionTools} PotionTools";
        return stringToPrint;
    }

    #region Operator

    /// <summary>
    ///     Checks if each Ressource-Properties of a is greater or equal to b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>true if every Ressource-Protery of a is greater or equal to those of b</returns>
    public static bool operator >=(Cost a, Cost b)
    {
        return a.Coins >= b.Coins && a.Wood >= b.Wood && a.Stone >= b.Stone &&
               a.PotionTools >= b.PotionTools;
    }

    /// <summary>
    ///     Checks if each Ressource-Properties of a is less or equal to b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>true if every Ressource-Protery of a is less or equal to those of b</returns>
    public static bool operator <=(Cost a, Cost b)
    {
        return a.Coins <= b.Coins && a.Wood <= b.Wood && a.Stone <= b.Stone &&
               a.PotionTools <= b.PotionTools;
    }

    public static Cost operator /(Cost a, float b)
    {
        return new Cost
        {
            Coins = a.Coins / b,
            Wood = a.Wood / b,
            Stone = a.Stone / b,
            PotionTools = a.PotionTools / b
        };
    }

    /// <summary>
    ///     Adds each single Ressource-Property of the two Cost-Objects together.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>New Cost-Object with added Ressource Properties</returns>
    public static Cost operator +(Cost a, Cost b)
    {
        return new Cost
        {
            Coins = a.Coins + b.Coins,
            Wood = a.Wood + b.Wood,
            Stone = a.Stone + b.Stone,
            PotionTools = a.PotionTools + b.PotionTools
        };
    }

    /// <summary>
    ///     Substarcts each single Ressource-Property of the two Cost-Objects together.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>New Cost-Object with substracted Ressource Properties</returns>
    public static Cost operator -(Cost a, Cost b)
    {
        return new Cost
        {
            Coins = a.Coins - b.Coins,
            Wood = a.Wood - b.Wood,
            Stone = a.Stone - b.Stone,
            PotionTools = a.PotionTools - b.PotionTools
        };
    }

    #endregion

    #region Standard-Function

    public int CompareTo(Cost other)
    {
        if (other == null) return 1;

        var thisTotal = Coins + Wood + Stone + PotionTools;
        var otherTotal = other.Coins + other.Wood + other.Stone + other.PotionTools;

        return thisTotal.CompareTo(otherTotal);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Coins, Wood, Stone, PotionTools);
    }

    public override string ToString()
    {
        return $"Cost(Coins: {Coins}, Wood: {Wood}, Stone: {Stone}, PotionTools: {PotionTools})";
    }

    public override bool Equals(object obj)
    {
        if (obj is not Cost other) return false;
        if (!Coins.Equals(other.Coins)) return false;
        if (!Wood.Equals(other.Wood)) return false;
        if (!Stone.Equals(other.Stone)) return false;
        if (!PotionTools.Equals(other.PotionTools)) return false;
        return true;
    }

    public Cost AddAbsolute(Cost other)
    {
        return this + other;
    }

    public Cost ApplyPercent(Cost pct)
    {
        return new Cost
        {
            Coins = Coins * (1 + pct.Coins),
            Wood = Wood * (1 + pct.Wood),
            Stone = Stone * (1 + pct.Stone),
            PotionTools = PotionTools * (1 + pct.PotionTools)
        };
    }
    public Cost ApplyPercent(float pct) {

        return new Cost
        {
            Coins = Coins * (1 + pct),
            Wood = Wood * (1 + pct),
            Stone = Stone * (1 + pct),
            PotionTools = PotionTools * (1 + pct)
        };
    }
    


    #endregion
}