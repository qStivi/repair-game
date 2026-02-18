public class MainBuilding : AbstractProductionBuilding, IStorage
{
    protected float productionFaktor;

    protected override void Start()
    {
        base.Start();
        EventManager.OnUpdatedStorage();
    }

    public Cost StorageCapacity { get; protected set; }

    /// <summary>
    ///     Das Hauptgebäude produziert immer Coins mittels eines gewissen Faktors der mit den momentan
    ///     vorhandenen Coins
    ///     verrechnet wird.
    /// </summary>
    protected override void ProductionTimerElapsed()
    {
        var productionToAdd = ressourcesOfLevel.Coins * productionFaktor;
        LevelDataMananger.Instance.AddRessources(new Cost((long)productionToAdd));
    }
}