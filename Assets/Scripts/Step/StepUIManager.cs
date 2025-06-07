// StepUIManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StepUIManager : MonoBehaviour
{
    public static StepUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI totalStepsText;
    [SerializeField] private TextMeshProUGUI workoutStepsText;
    [SerializeField] private Button resetTotalStepsButton;

    private int workoutSteps = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        UpdateTotalStepsDisplay(0);
        UpdateWorkoutStepsDisplay(0);

        resetTotalStepsButton?.onClick.AddListener(OnResetTotalStepsButtonClicked);
    }

    private void OnEnable()
    {
        StepTracker.OnStepAdded += OnWorkoutStepAdded;
    }

    private void OnDisable()
    {
        StepTracker.OnStepAdded -= OnWorkoutStepAdded;
    }

    public void UpdateTotalStepsDisplay(int totalSteps)
    {
        totalStepsText.text = $"Total Steps: {totalSteps}";
    }

    public void UpdateWorkoutStepsDisplay(int steps)
    {
        workoutStepsText.text = $"Workout Steps: {steps}";
    }

    private void OnWorkoutStepAdded(int addedSteps)
    {
        workoutSteps += addedSteps;
        UpdateWorkoutStepsDisplay(workoutSteps);
    }

    // Made public so it can be called from WorkoutControlUIManager
    public void ResetWorkoutSteps()
    {
        workoutSteps = 0;
        UpdateWorkoutStepsDisplay(workoutSteps);
    }

    private void OnResetTotalStepsButtonClicked()
    {
        StepTracker.Instance?.DebugResetTotalSteps();
    }
}
