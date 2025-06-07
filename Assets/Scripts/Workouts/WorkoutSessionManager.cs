// WorkoutSessionManager.cs
using System.Collections.Generic;
using UnityEngine;

public class WorkoutSessionManager : MonoBehaviour
{
    public static WorkoutSessionManager Instance { get; private set; }

    public event System.Action OnCompanionLeveledUp;
    public event System.Action OnWorkoutEnded;

    public List<CompanionCardUI> AllCards = new List<CompanionCardUI>();

    public void RegisterCard(CompanionCardUI card)
    {
        if (!AllCards.Contains(card))
            AllCards.Add(card);
    }

    public bool ToggleSelection(CompanionData companion)
    {
        // If already selected, unselect it
        if (SelectedCompanion == companion)
        {
            SelectedCompanion = null;
            CompanionUIManager.Instance.ClearDisplay();
            foreach (var card in AllCards)
                card?.SetSelected(false);
            return false;
        }

        // Otherwise, select the new one
        SelectedCompanion = companion;
        CompanionUIManager.Instance.SetDisplay(companion);
        foreach (var card in AllCards)
            card?.SetSelected(card.Companion == companion);

        return true;
    }

    public CompanionData SelectedCompanion { get; private set; }
    public bool IsWorkoutActive { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void StartWorkout()
    {
        if (SelectedCompanion == null) return;

        Debug.Log("Workout started");
        IsWorkoutActive = true;
        SeedGrowthManager.Instance.ResetSeedProgress();
    }

    public void EndWorkout()
    {
        Debug.Log("Workout ended");
        IsWorkoutActive = false;
        SeedGrowthManager.Instance.ResetSessionSeedProgress();
        OnWorkoutEnded?.Invoke();
        // Show summary later
    }

    public void ResetSessionXP()
    {
        if (SelectedCompanion != null)
            SelectedCompanion.ResetSessionXP();
    }

    public void NotifyCompanionLeveledUp()
    {
        OnCompanionLeveledUp?.Invoke();
    }
}
