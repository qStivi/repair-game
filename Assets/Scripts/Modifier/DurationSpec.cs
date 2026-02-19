public enum DurationType
{
    Instant,        // apply once
    Timed,          // lasts seconds
    Permanent,      // never expires
    UntilEvent      // expires on some game event
}
public enum ExpireEvent
{
    EndOfTurn,
    EndOfDay,
    BuildingDestroyed,
    LeaveArea
}
[System.Serializable]
public struct DurationSpec
{
    public DurationType Type;
    public float? Seconds;         // only used if Type == Timed
    public ExpireEvent? ExpireOn;  // only used if Type == UntilEvent
    public DurationSpec(DurationType type, float? seconds, ExpireEvent? expireOn)
    {
        this.Type = type;
        this.Seconds = seconds;
        this.ExpireOn = expireOn;
    }
}