// WorkoutControlUIManager.cs
using UnityEngine;
using UnityEngine.UI;

public class WorkoutControlUIManager : MonoBehaviour
{
    public Button startButton;
    public Button endButton;

    void Start()
    {
        startButton.onClick.AddListener(StartWorkout);
        endButton.onClick.AddListener(EndWorkout);
        UpdateUIState();
    }

    void Update()
    {
        UpdateUIState();
    }

    void StartWorkout()
    {
        WorkoutSessionManager.Instance?.StartWorkout();
    }

    void EndWorkout()
    {
        WorkoutSessionManager.Instance?.EndWorkout();
        // Reset the workout?only counter on the UI
        StepUIManager.Instance?.ResetWorkoutSteps();
    }

    void UpdateUIState()
    {
        var manager = WorkoutSessionManager.Instance;
        if (manager == null)
        {
            startButton.interactable = false;
            endButton.interactable = false;
            return;
        }
        startButton.interactable = manager.SelectedCompanion != null && !manager.IsWorkoutActive;
        endButton.interactable = manager.IsWorkoutActive;
    }
}
