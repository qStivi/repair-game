public class StoneMason : AbstractProductionBuilding, IStorage
{
    protected override void Start()
    {
        base.Start();
        EventManager.OnUpdatedStorage();
    }

    public Cost StorageCapacity { get; protected set; }
}