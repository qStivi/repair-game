/// <summary>
/// Basis Interface für alles mit einer ID
/// </summary>
public interface IIdentifiable<out T>
{
    T UniqueId { get; }
}