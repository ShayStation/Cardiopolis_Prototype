using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    public static CompanionManager Instance { get; private set; }

    [Header("Available Companion Types")]
    public List<CompanionType> AvailableTypes; // Assign in inspector

    [Header("Your Companions")]
    public List<CompanionData> OwnedCompanions = new List<CompanionData>();

    public delegate void CompanionLevelUpEvent(CompanionData companion);
    public event CompanionLevelUpEvent OnCompanionLeveledUp;


    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject); // Optional, keep across scenes
    }

    void Start()
    {
        if (OwnedCompanions.Count == 0)
        {
            GenerateStarterCompanions();
        }
    }

    public void GenerateStarterCompanions()
    {
        OwnedCompanions.Clear();

        if (AvailableTypes == null || AvailableTypes.Count == 0)
        {
            Debug.LogWarning("No CompanionTypes available to generate companions.");
            return;
        }

        foreach (var type in AvailableTypes)
        {
            var companion = CompanionGenerator.Generate(type);
            OwnedCompanions.Add(companion);
            Debug.Log($"Starter companion added: {companion.GeneratedName} the {type.TypeName}");
        }
    }

    /// <summary>
    /// Generate and add a new companion of a given type.
    /// </summary>
    public CompanionData GenerateCompanion(CompanionType type)
    {
        CompanionData newCompanion = new CompanionData(type);
        OwnedCompanions.Add(newCompanion);
        Debug.Log($"Generated new companion: {newCompanion.GeneratedName} the {type.TypeName}");
        return newCompanion;
    }

    /// <summary>
    /// Add XP to a companion by index in the list.
    /// </summary>
    public void AddXPToCompanion(int index, float xp)
    {
        if (index < 0 || index >= OwnedCompanions.Count)
            return;

        var companion = OwnedCompanions[index];
        float prevLevel = companion.Level;

        companion.AddXP(xp);

        if (companion.Level > prevLevel)
        {
            OnCompanionLeveledUp?.Invoke(companion);
        }
    }

    /// <summary>
    /// Clear all XP and reset level of all companions.
    /// </summary>
    public void ResetAllCompanions()
    {
        foreach (var companion in OwnedCompanions)
        {
            companion.Reset();
        }
    }

    /// <summary>
    /// Remove all companions from the list.
    /// </summary>
    public void ClearCompanionList()
    {
        OwnedCompanions.Clear();
    }
}
