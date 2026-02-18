/// <summary>
///     All mutable LevelData
/// </summary>
public class LevelState
{
    public Cost ressources;

    public LevelState(LevelConfig config)
    {
        ressources = config.startingRessources;
    }
}