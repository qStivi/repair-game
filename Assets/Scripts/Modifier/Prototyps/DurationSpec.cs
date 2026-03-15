using UnityEngine;

public enum DurationType
{
    Instant,        // apply once
    Timed,          // lasts seconds
    Permanent,      // never expires
    UntilEvent      // expires on some game event
}
public enum ExpireEvent
{
    None,
    EndOfTurn,
    EndOfDay,
    BuildingDestroyed,
    LeaveArea
}
[System.Serializable]
public struct DurationSpec
{
    public DurationType Type;
    //Cant make them Nullable because it wouldnt be shown in the Inspektor.
    public float Seconds;         // only used if Type == Timed
    public ExpireEvent ExpireOn;  // only used if Type == UntilEvent
    public DurationSpec(DurationType type, float seconds = 0f, ExpireEvent expireOn = ExpireEvent.None)
    {
        this.Type = type;
        this.Seconds = seconds;
        this.ExpireOn = expireOn;
    }
    /// <summary>
    /// Creates a string signature out of Type and if necessary additional Data to be used for DataComparison
    /// </summary>
    public string ToSignatureString()
    {
        switch (Type)
        {
            case DurationType.Instant:
                return "instant";

            case DurationType.Permanent:
                return "permanent";

            case DurationType.Timed:
                {
                    // Round to milliseconds precision Math.f Unity Engine for floats
                    float s = Mathf.Round(Seconds * 1000f) / 1000f;
                    return $"timed:{s}";
                }

            case DurationType.UntilEvent:
                return $"event:{ExpireOn}";

            default:
                return "unknown";
        }
    }
}