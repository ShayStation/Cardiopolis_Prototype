
using UnityEngine;
using System.Runtime.InteropServices;

public class StepTrackerManager : MonoBehaviour
{
    public static StepTrackerManager Instance { get; private set; }

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StartStepTracking();

    [DllImport("__Internal")]
    private static extern void StopStepTracking();
#else
    private static void StartStepTracking() => Debug.Log("StartStepTracking (stub)");
    private static void StopStepTracking() => Debug.Log("StopStepTracking (stub)");
#endif

    public int CurrentStepCount { get; private set; }
    public event System.Action<int> OnStepUpdated;

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void GetStepCountToday();
#else
    private static void GetStepCountToday() => Debug.Log("GetStepCountToday (stub)");
#endif

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartStepTracking(); // Ensure it starts once at launch
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            StopStepTracking();
            Instance = null;
        }
    }

    public void OnStepUpdate(string stepCountStr)
{
    Debug.Log("OnStepUpdate received: " + stepCountStr);

    if (int.TryParse(stepCountStr, out int steps))
    {
        int previousSteps = StepTracker.Instance?.TotalSteps ?? 0;

        CurrentStepCount = steps;
        OnStepUpdated?.Invoke(steps);

        // Sync with in-game step tracker and update UI
        if (StepTracker.Instance != null)
        {
            StepTracker.Instance.TotalSteps = steps;
            StepUIManager.Instance?.UpdateTotalStepsDisplay(steps);
        }

        // Workout-specific XP and seed gain logic
        if (WorkoutSessionManager.Instance != null &&
            WorkoutSessionManager.Instance.IsWorkoutActive &&
            steps > previousSteps)
        {
            int delta = steps - previousSteps;

            float xpGain = delta * 1f;
            WorkoutSessionManager.Instance.SelectedCompanion?.AddXP(xpGain);

            // Fire step-based seed growth event
            StepTracker.RaiseOnStepAdded(delta);

        }
    }
    else
    {
        Debug.LogWarning("Invalid step data: " + stepCountStr);
    }
}


    public void DebugRequestStepCheck()
    {
        Debug.Log("Requesting manual step check...");
        GetStepCountToday();
    }

}
