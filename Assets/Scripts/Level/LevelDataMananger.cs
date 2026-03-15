using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


/// <summary>
///     Ein Manager für LevelData nach Singleton-Prinzip:
///     Singleton stellt sicher, dass es genau eine Instanz einer Klasse gibt,
///     auf die du von überall im Code aus über eine statische Property zugreifen kannst.
/// </summary>
public class LevelDataMananger : MonoBehaviour
{
    [Header("LevelConfig")] public LevelConfig config;

    [HideInInspector] public LevelState state;

    public static LevelDataMananger Instance { get; private set; }

    public ModifierSystem modifierSystem = new();

    public StringIdGenerator stringIdGenerator = new();

    // private readonly Timer syncTickEverySecond = new(1000);

    [Tooltip("How much max space for ressources is given.")]
    public Cost StorageCapacity { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        state = new LevelState(config);
    }

    private void Start()
    {
        //syncTickEverySecond.AutoReset = true;
        //syncTickEverySecond.Elapsed += EventManager.OnSyncTickEverySecond;
        //syncTickEverySecond.Enabled = true;
        StartSyncTick();

        EventManager.UpdateStorage += UpdateStorageCapazity;
    }


    private void OnDisable()
    {
        //syncTickEverySecond.Dispose();
        EventManager.UpdateStorage -= UpdateStorageCapazity;
    }

    //UIComponents
    public event Action<Cost> OnLevelRessourcesChanged;

    public void AddRessources(Cost cost)
    {
        state.ressources += cost;
        OnLevelRessourcesChanged?.Invoke(state.ressources);
    }

    public void RemoveRessources(Cost cost)
    {
        state.ressources -= cost;
        OnLevelRessourcesChanged?.Invoke(state.ressources);
    }


    /// <summary>
    ///     Jede Sekunde wird ein SyncTick für alle Timer ausgerufen. Verwendung von Awaitable, um
    ///     generelle Synco mit Unity Frames zu erhalten.
    /// </summary>
    /// <returns>Awaitable, um theoretisch Status der Methode abfragen zu können</returns>
    private async Awaitable StartSyncTick()
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            Debug.Log("While sync entered");
            await Awaitable.WaitForSecondsAsync(1.0f, destroyCancellationToken);
            EventManager.OnSyncTickEverySecond();
        }
    }

    private void UpdateStorageCapazity()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Cost completeStorage = new();
        var buildingsWithStorage = (List<IStorage>)InterfaceHelper.FindOnActiveScene<IStorage>();
        foreach (var storage in buildingsWithStorage)
            if (storage != null)
                completeStorage += storage.StorageCapacity;

        stopwatch.Stop();
        Debug.Log(
            $"Die momentane MaximaleKapzität ist: {completeStorage}. Berechnet in {stopwatch.ElapsedMilliseconds}");
        StorageCapacity = completeStorage;
    }

    #region Vermutlich sinnvoll in LevelKonfig zu überführen

    [field: SerializeField]
    [Tooltip("How much it costs to unlock a BuildingSpot.")]
    public long BuildingSpotUnlockCost { get; private set; }

    [field: SerializeField]
    [Tooltip(
        "The time in seconds it takes to unlock a BuildingSpot after a building has been destroyed.")]
    public float BuildingSpotCooldown { get; private set; }

    #endregion
}