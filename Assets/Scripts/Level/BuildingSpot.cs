using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [field: SerializeField]
    [Tooltip("Weather or not this spot has initially been unlocked.")]
    public bool Blocked { get; private set; }

    [field: SerializeField]
    [Tooltip("Weather this spot is blocked due to a building being destroyed.")]
    public bool Locked { get; private set; }

    [field: SerializeField]
    [Tooltip(
        "The time remaining in seconds it takes unlock this spot after a building has been destroyed.")]
    public float Cooldown { get; private set; }

    public TMP_Dropdown _dropdown;

    [Obsolete("Warning: The type of this property may change in the future.")]
    [field: SerializeField]
    public List<GameObject> PossibleBuildings { get; set; } = new();

    private LevelDataMananger _levelData;

    private void Start()
    {
        // Set level data instance by searching for the LevelData script in the scene by tag
        _levelData = GameObject.FindWithTag("LevelData").GetComponent<LevelDataMananger>();

        _dropdown.options.Clear();
        foreach (var building in PossibleBuildings)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData(building.name));
            _dropdown.onValueChanged.AddListener(delegate { BuildBuilding(); });
        }
    }

    private void Update()
    {
        if (!Locked) return;
        Cooldown -= Time.deltaTime;
        if (Cooldown <= 0) Locked = false;
    }

    /// <summary>
    ///     Indicates if this spot is currently occupied by a building.
    /// </summary>
    /// <returns>True if the spot is occupied, false otherwise.</returns>
    private bool IsOccupied()
    {
        // If this GameObject has children
        return transform.childCount > 0;
    }

    /// <summary>
    ///     Sets the building spot to locked and starts the cooldown timer.
    /// </summary>
    public void BuildingDestroyed()
    {
        Locked = true;
        Cooldown = _levelData.BuildingSpotCooldown;
    }

    public void BuildBuilding()
    {
        Debug.Log("Building building");
        if (IsOccupied() || Locked) return;
        // Instantiate the selected building
        var building = Instantiate(PossibleBuildings[_dropdown.value], transform.position,
            Quaternion.identity);
        // Set the parent of the building to this building spot
        building.transform.SetParent(transform);
        // Set the position of the building to the position of this building spot
        building.transform.position = transform.position;
        // Set the building spot to locked
        Locked = true;
    }
}