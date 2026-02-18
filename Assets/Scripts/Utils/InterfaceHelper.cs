using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public static class InterfaceHelper
{
    public static IEnumerable<T> FindOnActiveScene<T>(bool includeInactive = false)
    {
        return SceneManager.GetActiveScene().GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
    }
}