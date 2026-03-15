
/// <summary>
/// Beschreibt alle GameObjekte, deren Werte durch Modifikationen verändert werden können. 
/// </summary>
public interface IModifierTarget : IIdentifiable<int>
{
   // ModifierSystem ModifierSystem { get; }
    ModifierDomain Domain { get; }
    int TargetId { get; }

    // Mapping: UniqueId == TargetId
    int IIdentifiable<int>.UniqueId => TargetId;
}
