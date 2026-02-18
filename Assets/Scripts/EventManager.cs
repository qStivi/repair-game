using System;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public delegate RessourceEventArgs RessourceEventHandler();

    #region SyncronisationsTick für jede Sekunde

    public static event UnityAction SyncTickEverySecond;

    public static void OnSyncTickEverySecond()
    {
        Debug.Log("SyncTickInvoked");
        SyncTickEverySecond?.Invoke();
    }

    #endregion

    #region Update Storage

    public static event UnityAction UpdateStorage;

    public delegate Cost StorageEventHandler(); //veraltet

    public static void OnUpdatedStorage()
    {
        UpdateStorage?.Invoke();
    }

    #endregion

    #region Update Ressorce momentan �ber direkte Verweise

    public static event RessourceEventHandler UpdateRessources;

    public static void OnRessourceUpdates(object sender, RessourceEventArgs e)
    {
        UpdateRessources?.Invoke();
    }
}

public class RessourceEventArgs : EventArgs
{
    public Cost RessourceCosts { get; set; }
}

#endregion

public class StorageEventArgs : EventArgs
{
    public Cost Storage { get; set; }
}