using TMPro;
using UnityEngine;

public abstract class AbstractProductionBuilding : AbstractBuilding
{
    [Header("ProductionBuildingUI")] public TextMeshPro productionTick;

    public Animator animatorOfProductionText;
    protected Stat<Cost> productionAmount;

    protected Stat<FloatValue> productionIntervall;

    private TimerManager productionTimer;
    private string textProductionTick;

    protected override void Start()
    {
        base.Start();
        animatorOfProductionText = productionTick.GetComponent<Animator>();
    }

    protected override void LoadStats()
    {
        base.LoadStats();
        productionIntervall = (Stat<FloatValue>)stats[StatTypeBuilding.ProductionIntervall];
        productionAmount = (Stat<Cost>)stats[StatTypeBuilding.ProductionAmount];
    }

    protected override void InitiateTimers()
    {
        base.InitiateTimers();
        productionTimer = new TimerManager(productionIntervall.GetValue(), true);
        productionTimer.TimerElapsed += ProductionTimerElapsed;
        productionTimer.StartTimer();
    }


    protected virtual void ProductionTimerElapsed()
    {
        Debug.Log("ProductionTimerElapsed");
        textProductionTick = $"+ {productionAmount.GetValue().ToStringIfNotZeroOrNull()}";
        UIChanged = true;
        LevelDataMananger.Instance.AddRessources(productionAmount.GetValue());

        //EventManager.OnRessourceUpdates(this, new RessourceEventArgs { RessourceAmount = productionAmount, RessourceType = ressourceType });       
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        productionTick.text = textProductionTick;
        if (animatorOfProductionText != null)
        {
            Debug.Log("Animation if entered");
            animatorOfProductionText.Play("FadeOut");
        }
    }

    protected override void DestroyBuilding()
    {
        productionTimer.Stop();
        //Handle StorageCapazity
        base.DestroyBuilding();
    }
}