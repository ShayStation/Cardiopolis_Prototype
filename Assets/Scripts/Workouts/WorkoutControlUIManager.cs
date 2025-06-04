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
        // Refresh button state based on selection and activity
        UpdateUIState();
    }

    void StartWorkout()
    {
        WorkoutSessionManager.Instance?.StartWorkout();
    }

    void EndWorkout()
    {
        WorkoutSessionManager.Instance?.EndWorkout();
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
