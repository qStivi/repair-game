
using System;

public readonly struct StackKey : IEquatable<StackKey>
{
    public readonly string protoId;
    public readonly string sourceId;
    public readonly int targetId; // 0 if global stack across targets
  

    public StackKey(string protoId, string sourceId, int? targetId)
    {
        this.protoId = protoId;
        this.targetId = targetId ?? 0;
        this.sourceId = sourceId;
    }

    public bool Equals(StackKey other) =>
        protoId == other.protoId && targetId == other.targetId && sourceId == other.sourceId;

    public override int GetHashCode() => HashCode.Combine(protoId, sourceId, targetId);
}
