using System;


public struct StatBucketKey : IEquatable<StatBucketKey>
{
    public ModifierDomain Domain;
    // Reference to the stat id, which is unique for each statType enum value within a given domain. 
    // This is used to identify the stat type in way that not using the enum directly to support different stat types for different domains.
    public int statId;
    public int targetId; //0 if global

    public StatBucketKey(ModifierDomain domain, int statId, int targetId = 0)
    {
        Domain = domain;
        this.statId = statId;
        this.targetId = targetId;
    }

    public readonly bool Equals(StatBucketKey other)
    {
        return Domain.Equals(other.Domain) && statId == other.statId && targetId == other.targetId;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine((int)Domain, statId, targetId);
    }
}
