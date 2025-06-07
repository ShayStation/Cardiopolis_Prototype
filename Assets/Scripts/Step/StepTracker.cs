using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class StepTracker : MonoBehaviour
{
    public static StepTracker Instance { get; private set; }

    public int TotalSteps = 0;
    public int StepsPerPress = 100;
    private Controls controls;

    public delegate void OnStepChanged(int steps);
    public static event OnStepChanged OnStepAdded;

    private void Awake()
    {
        Instance = this;
        controls = new Controls();
        controls.Gameplay.SimulateStep.performed += ctx => AddSteps();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    public void DebugAddSteps()
    {
        AddSteps();
    }

    private void AddSteps()
    {
        TotalSteps += StepsPerPress;
        Debug.Log($"Steps: {TotalSteps}");

        // Update the persistent total counter
        StepUIManager.Instance?.UpdateTotalStepsDisplay(TotalSteps);

        // Grant XP (full rate if in?workout, quarter rate otherwise)
        float xpGain = StepsPerPress *
            (WorkoutSessionManager.Instance != null && WorkoutSessionManager.Instance.IsWorkoutActive
                ? 1f
                : 0.25f);

        if (WorkoutSessionManager.Instance?.SelectedCompanion != null)
        {
            WorkoutSessionManager.Instance.SelectedCompanion.AddXP(xpGain);
        }

        // Fire the workout?only event
        if (WorkoutSessionManager.Instance != null && WorkoutSessionManager.Instance.IsWorkoutActive)
        {
            OnStepAdded?.Invoke(StepsPerPress);
        }
    }

    /// <summary>
    /// Debug: reset the entire step history to zero.
    /// </summary>
    public void DebugResetTotalSteps()
    {
        TotalSteps = 0;
        Debug.Log("Total Steps reset to 0");
        StepUIManager.Instance?.UpdateTotalStepsDisplay(TotalSteps);
    }
}
