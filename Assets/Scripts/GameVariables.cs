using UnityEngine;

public class GameVariables : MonoBehaviour
{
    // Public static instance of GameVariables for use in other scripts and scenes
    public static GameVariables Instance;

    private void Awake()
    {
        HandlePersistantGameObject();
    }

    private void HandlePersistantGameObject()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Global Variables

    // Used for almost everything in the MainMenuShop
    public long MainCurrency { get; set; } = 0;

    // For special use in MainMenuShop
    public long SecondaryCurrency { get; set; } = 0;

    #endregion
}