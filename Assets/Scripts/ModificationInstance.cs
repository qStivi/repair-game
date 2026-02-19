using System;
using System.Collections.Generic;
/// <summary>
/// Interface für Spielmechaniken, die Modifikation Prototypen enthalten, also aus der Modifier entstehen können (z.B. Cards oder Potions)
/// </summary>
public interface IModificationSource
{
    List<ModifierPrototype> ModifierPrototyps { get; }
}

public class ModifierPrototype
{

}


public interface IModificationInstance
{
    bool AppliesTo<T>(T target);
}



/// <summary>
///     Alle Modifiers sowie die entsprechend für die Modifier zuständige Spielmechanik z.B. Cards or Potions
/// </summary>
public class ModificationInstance<TargetType> 
{
    //Funktion bestimmt, für welche Typen die Filter gelten also z.B. b => b.card.category.HasFlag(...)
    private readonly Func<TargetType, bool> _filter;

    //Alle Runtime-Modifier, die aus den Card oder Spielmechanik-Daten erzeugt wurden
    private readonly List<Modifier<object>> _modifiers = new();

    private readonly IModificationSource modSource;

    public ModificationInstance(IModificationSource modificationSource, Func<TargetType, bool> appliesTo)
    {
        this.modSource  = modificationSource;
        _filter = appliesTo;

        // Erzeuge Runtime-Modifier (also Modifier<T>) aus allen Prototypes, zugrundeliegenden Daten (Aufbau noch abzuklären)
           foreach (var proto in modificationSource.ModifierPrototyps)
           {
               // Beispiel: proto.CreateRuntimeModifier(this) liefert Modifier<T>
               //_modifiers.Add(proto.CreateRuntimeModifier(this));

           }
    }

    public bool AppliesTo(TargetType target) => _filter(target);

    public IEnumerable<Modifier<object>> GetModifiers() => _modifiers.AsReadOnly();
}