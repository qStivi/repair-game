using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractBuilding : MonoBehaviour, IModifierTarget
{
    public BuildingDefinition definition;
    public int currentLevel = 1;
    public ModifierDomain Domain => ModifierDomain.Building;
    //FÜr den Anfang über simplen StringGenerator aus LevelData.Instance zu setzen 
    public int TargetId {  get; protected set; }

    //ModifierSystem IModifierTarget.ModifierSystem => LevelDataMananger.Instance.modifierSystem;


    [Header("UI")] public bool UIChanged;

    [SerializeField] protected TextMeshPro repairTimerUI;

    [SerializeField] protected Image radialIndicatorUI;

    //Building
    protected Stat<FloatValue> buildDuration = new(0f);
    protected Stat<Cost> buildingCosts;
    protected BuildingLevelData dataCurrentLevel;
    protected Stat<FloatValue> hitpoints;
    protected Stat<Cost> repairCost;

    protected Stat<FloatValue> repairDuration = new(0f);

    //Repair
    protected Stat<FloatValue> repairIntervall;

    /*[Header("General Values of Buildings")]
    [SerializeField]
    protected int hitpoints;

    [SerializeField]
    [Tooltip("Wie lange muss der Spieler den Bildschirm berühren bis der Bauprozess abgeschlossen ist. Meistens erfolgt dies direkt.")]
    protected float buildDuration;

    [SerializeField]
    [Tooltip("Das Intervall in dem das Gebäude spätestens repariert werden muss, sonst wird es zerstört.")]
    protected double repairIntervall;

    protected Cost buildingCosts;
    protected int currentLevel = 1;
    protected Cost repairCost;
    protected float repairDuration = 0;*/

    protected TimerManager repairIntervallTimer;

    [Header("Verweise")] protected Cost ressourcesOfLevel;

    [Header("Stats")] protected Dictionary<StatTypeBuilding, IStat> stats;



    protected virtual void Awake()
    {
        ressourcesOfLevel = LevelDataMananger.Instance.state.ressources;
        LoadStats();
    }

    protected virtual void Start()
    {
        InitiateTimers();
    }

    protected virtual void Update()
    {
        repairTimerUI.text = repairIntervallTimer.TimeRemaining.ToString();
        radialIndicatorUI.fillAmount =
            repairIntervallTimer.TimeRemaining / repairIntervall.GetValue();
        if (UIChanged)
            UpdateUI();
    }

    protected virtual void LoadStats()
    {
        dataCurrentLevel = definition.levels.First(lvl => lvl.level == currentLevel);
        stats = dataCurrentLevel.stats.ToDictionary(stat => stat.StatType,
            stat => stat.CreateStat());
        //Können Werte, die eigentlich nur einmal benötigt werden überhaupt in Stats gespeichert werden bspw. BuildingCost wobei es kann ja wieder alles beeinflusst werden.
        hitpoints = (Stat<FloatValue>)stats[StatTypeBuilding.Hitpoints];
        buildDuration = (Stat<FloatValue>)stats[StatTypeBuilding.BuildDuration];
        buildingCosts = (Stat<Cost>)stats[StatTypeBuilding.BuildingCosts];

        repairIntervall = (Stat<FloatValue>)stats[StatTypeBuilding.RepairIntervall];
        repairDuration = (Stat<FloatValue>)stats[StatTypeBuilding.RepairDuration];
        repairCost = (Stat<Cost>)stats[StatTypeBuilding.RepairCost];
    }

    protected virtual void UpdateUI()
    {
        UIChanged = false;
    }

    protected virtual void RepairBuilding()
    {
        if (repairCost.GetValue() <= ressourcesOfLevel)
        {
            ressourcesOfLevel -= repairCost.GetValue();
            repairIntervallTimer.ResetTimer();
        }
    }

    protected bool IsMaxLevel()
    {
        return dataCurrentLevel.levelUpCost == null;
    }

    protected bool IsUpdatePossible()
    {
        return !IsMaxLevel() && ressourcesOfLevel >= dataCurrentLevel.levelUpCost;
    }

    //Ausstehend, was passiert mit Timern und ggf. übernahme aktueller Werte, oder einfach alle reseten auch neue Werte bei Update
    protected void UpdateBuilding()
    {
        if (IsUpdatePossible())
        {
            LevelDataMananger.Instance.RemoveRessources(dataCurrentLevel.levelUpCost);
            currentLevel++;
            LoadStats();
        }
    }

    protected virtual void InitiateTimers()
    {
        repairIntervallTimer = new TimerManager(repairIntervall.GetValue());
        repairIntervallTimer.TimerElapsed += DestroyBuilding;
        repairIntervallTimer.StartTimer();
    }

    protected virtual void DestroyBuilding()
    {
        repairIntervallTimer.Stop();
        Destroy(gameObject);
        //Call buildingSpotClass to start Countdown
        transform.parent.GetComponent<BuildingSpot>().BuildingDestroyed();
    }
}