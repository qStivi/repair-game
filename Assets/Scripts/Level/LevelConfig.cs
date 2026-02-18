using UnityEngine;

/// <summary>
///     Die einzelnen Level sind Gebieten zugewiesen, die nach und nach auch der "Weltkarte"
///     freigeschaltet werden.
/// </summary>
public enum Area
{
    Starting
}


[CreateAssetMenu(menuName = "Game/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public string levelId;
    public string levelName;
    public string levelDescription;
    public Area area;

    public Cost startingRessources;
    //Viele weitere Settings wie Spawnwahrscheinlichkeiten von Gegnern, welche Gegner spawnen überhaupt, wie weit für die Scouts zum nächsten Level usw.
}