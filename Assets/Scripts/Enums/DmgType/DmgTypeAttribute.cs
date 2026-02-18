using System;

[AttributeUsage(AttributeTargets.Field)]
public class DmgTypeAttribute : Attribute
{
    public DmgTypeAttribute(DmgCategory category)
    {
        Category = category;
    }

    public DmgCategory Category { get; }
}

public enum DmgCategory
{
    Physical,
    Elemental,
    Magical
}