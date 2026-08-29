using System.Collections.Generic;


/// <summary>
/// Interface für Spielmechaniken, die Modifikation Prototypen enthalten, also aus denen Modifier entstehen können (z.B. Cards oder Potions)
/// </summary>
public interface IModificationSource : IIdentifiable<string>
{
    List<ModifierPrototypeSO> ModifierPrototyps { get; }
    string SourceId { get; }

    string IIdentifiable<string>.UniqueId => SourceId;
}
