using UnityEngine;

public class SeedGrowthManager : MonoBehaviour
{
    public float CurrentGrowth = 0f;
    public float GrowthRequired = 1000f;
    public int SeedsGrown = 0;
    public float GrowthPerStep = 0.5f;
    public int SessionSeedsGrown { get; private set; }

    public delegate void OnSeedComplete();
    public static event OnSeedComplete OnSeedFullyGrown;

    public static SeedGrowthManager Instance { get; private set; }
   


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        StepTracker.OnStepAdded += HandleStepAdded;
    }

    void OnDisable()
    {
        StepTracker.OnStepAdded -= HandleStepAdded;
    }

    void HandleStepAdded(int steps)
    {
        float growthGained = steps * GrowthPerStep;
        CurrentGrowth += growthGained;

        if (CurrentGrowth >= GrowthRequired)
        {
            CurrentGrowth = 0f;
            SeedsGrown++;

            if (WorkoutSessionManager.Instance != null && WorkoutSessionManager.Instance.IsWorkoutActive)
            {
                SessionSeedsGrown++;
            }

            Debug.Log($"Seed fully grown! Total: {SeedsGrown}, This session: {SessionSeedsGrown}");
            OnSeedFullyGrown?.Invoke();
        }
    }

    public void ResetSeedProgress()
    {
        CurrentGrowth = 0f;
        Debug.Log("Seed growth progress has been reset.");
    }

    public void ResetSessionSeedProgress()
    {
        SessionSeedsGrown = 0;
        Debug.Log("Session seed count has been reset.");
    }

}
