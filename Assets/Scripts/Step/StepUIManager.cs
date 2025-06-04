using UnityEngine;
using TMPro;

public class StepUIManager : MonoBehaviour
{
    public static StepUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI stepsText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateStepDisplay(int totalSteps)
    {
        if (stepsText != null)
        {
            stepsText.text = $"Steps: {totalSteps}";
        }
    }
}
