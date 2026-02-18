public enum ModificationType
{
    Absolute,
    Relativ_Additive,
    Relativ_Mulitplicative
}

/*
 * Beispiel für relaiv_multiplicative
 * Faktor1 = 1 + 0.20 = 1.20
Faktor2 = 1 + 0.10 = 1.10
Endwert = Basis * Faktor1 * Faktor2  // = Basis * 1.32

 Weitere Idee: Meta-Modifier: Additive Boni nochmal um x % verbessern
*/

public class Modifier<T>
{
    public Modifier(ModificationType type, T value, object source, float? expireTime = null)
    {
        Type = type;
        Value = value;
        Source = source;
        ExpireTime = expireTime;
    }

    public ModificationType Type { get; }
    public T Value { get; }

    public object
        Source
    {
        get;
    } // Referenz zu (Spielmechanik)-Objekt, dass den Modifier erzeugt hat => Entfernung aller Modifier möglich, wenn diese Objekt "zerstört" wird

    public float? ExpireTime { get; }

    public bool IsExpired(float now)
    {
        return ExpireTime.HasValue && ExpireTime.Value <= now;
    }
}