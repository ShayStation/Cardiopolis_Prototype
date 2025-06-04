using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class StepTracker : MonoBehaviour
{
    public int TotalSteps = 0;
    public int StepsPerPress = 100;
    private Controls controls;

    public delegate void OnStepChanged(int steps);
    public static event OnStepChanged OnStepAdded;

    void Awake()
    {
        controls = new Controls();
        controls.Gameplay.SimulateStep.performed += ctx => AddSteps();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    public void DebugAddSteps()
    {
        AddSteps();
    }

    void AddSteps()
    {
        TotalSteps += StepsPerPress;
        Debug.Log($"Steps: {TotalSteps}");

        StepUIManager.Instance?.UpdateStepDisplay(TotalSteps);

        float xpGain = StepsPerPress * (WorkoutSessionManager.Instance != null && WorkoutSessionManager.Instance.IsWorkoutActive ? 1.0f : 0.25f);

        if (WorkoutSessionManager.Instance?.SelectedCompanion != null)
        {
            WorkoutSessionManager.Instance.SelectedCompanion.AddXP(xpGain);
        }

        if (WorkoutSessionManager.Instance != null && WorkoutSessionManager.Instance.IsWorkoutActive)
        {
            OnStepAdded?.Invoke(StepsPerPress);
        }
    }

}
