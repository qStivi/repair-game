#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ModifierPrototypeDuplicateValidator
{
    public static void Validate(ModifierPrototypeSO changed)
    {
        if (changed == null) return;

        string changedSig = changed.GetEffectSignature();
        if (string.IsNullOrEmpty(changedSig)) return;

        // 1) Find all assets of type ModifierPrototypeSO in the project
        string[] guids = AssetDatabase.FindAssets("t:ModifierPrototypeSO");

        foreach (string guid in guids)
        {
            // 2) Convert GUID -> asset path (e.g. "Assets/Modifiers/X.asset")
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 3) Load the asset at that path as ModifierPrototypeSO
            ModifierPrototypeSO other = AssetDatabase.LoadAssetAtPath<ModifierPrototypeSO>(path);

            if (other == null || other == changed) continue;

            // 4) Compare effect signatures
            if (other.GetEffectSignature() == changedSig)
            {
                // pass 'other' so the warning is clickable and selects that asset
                Debug.LogWarning(
                    $"Duplicate modifier effect detected.\n" +
                    $"This: {AssetDatabase.GetAssetPath(changed)}\n" +
                    $"Other: {AssetDatabase.GetAssetPath(other)}\n" +
                    $"Signature: {changedSig}",
                    other
                );
                return;
            }
        }
    }
}
#endif
