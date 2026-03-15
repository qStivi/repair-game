using System;


public struct StatBucketKey : IEquatable<StatBucketKey>
{
    public ModifierDomain Domain;
    public int statId;
    public int targetId; //0 if global

    public StatBucketKey(ModifierDomain domain, int statId, int targetId = 0)
    {
        this.Domain = domain;
        this.statId = statId;
        this.targetId = targetId;
    }

    public readonly bool Equals(StatBucketKey other)
    {
        return this.Domain.Equals(other.Domain) && this.statId == other.statId && this.targetId == other.targetId;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine((int)Domain, statId, targetId);
    }
}
