using TMPro;
using UnityEngine;

public class LevelDataUI : MonoBehaviour
{
    [Header("UIComponents")] [SerializeField]
    private TMP_Text coinsUI;

    [SerializeField] private TextMeshProUGUI woodUI;
    [SerializeField] private TextMeshProUGUI stoneUI;

    private void OnEnable()
    {
        LevelDataMananger.Instance.OnLevelRessourcesChanged += SetRessourceUI;
        SetRessourceUI(LevelDataMananger.Instance.state.ressources);
    }

    private void OnDisable()
    {
        LevelDataMananger.Instance.OnLevelRessourcesChanged -= SetRessourceUI;
    }

    private void SetRessourceUI(Cost ressourcesOfLevel)
    {
        coinsUI.text = ressourcesOfLevel.Coins.ToString();
        woodUI.text = ressourcesOfLevel.Wood.ToString();
        stoneUI.text = ressourcesOfLevel.Stone.ToString();
    }
}