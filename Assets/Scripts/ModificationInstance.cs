using System;
using System.Collections.Generic;

public interface IModificationInstance
{
    bool AppliesTo<T>(T target);
}


/// <summary>
///     Alle Modifiers sowie die entsprechend für die Modifier zuständige Spielmechanik z.B. Cards or
///     Potions
/// </summary>
public class ModificationInstance<TargetType>
{
    //Funktion bestimmt, für welche Typen die Filter gelten also z.B. b => b.card.category.HasFlag(...)
    private readonly Func<TargetType, bool> _filter;

    /// Alle Runtime-Modifier, die aus den Card oder Spielmechanik-Daten erzeugt wurden
    private readonly List<Modifier<object>> _modifiers = new();

    private readonly Card
        card; //oder entsprechend andere Struktur aus der sich die List oder einzelne Modifier<T> ergeben

    public ModificationInstance(Card card, Func<TargetType, bool> appliesTo)
    {
        this.card = card;
        _filter = appliesTo;

        // Erzeuge Runtime-Modifier (also Modifier<T>) aus allen Prototypes, zugrundeliegenden Daten (Aufbau noch abzuklären)
        /*   foreach (var proto in card.ModifierPrototypes)
           {
               // Beispiel: proto.CreateRuntimeModifier(this) liefert Modifier\<T\>
               _modifiers.Add(proto.CreateRuntimeModifier(this));

           }*/
    }

    // public bool AppliesTo(TargetType target) => _filter(target);

    //public IEnumerable<Modifier<object>> GetModifiers() => _modifiers.AsReadOnly();
}