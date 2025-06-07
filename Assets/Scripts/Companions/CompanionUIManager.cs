using TMPro;
using UnityEngine;

public class CompanionUIManager : MonoBehaviour
{
    public static CompanionUIManager Instance { get; private set; }
    public static bool AllowSelectionDuringWorkout = true;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
    }

    public TextMeshProUGUI companionXPText;
    public TextMeshProUGUI companionLevelText;
    public TextMeshProUGUI levelUpMessage;
    public TextMeshProUGUI companionNameText;
    public TextMeshProUGUI companionRoleText;


    public CompanionData activeCompanion; // Reference to the selected companion

    void OnEnable()
    {
        if (CompanionManager.Instance != null)
            CompanionManager.Instance.OnCompanionLeveledUp += HandleLevelUp;
    }

    void OnDisable()
    {
        if (CompanionManager.Instance != null)
            CompanionManager.Instance.OnCompanionLeveledUp -= HandleLevelUp;
    }

    void Update()
    {
        var active = WorkoutSessionManager.Instance?.SelectedCompanion;

        if (active != null)
        {
            companionNameText.text = active.GeneratedName;
            companionRoleText.text = active.RoleName;

            companionXPText.text = $"XP: {active.XP:F1} / {active.XPToNextLevel:F1}";
            companionLevelText.text = $"Level: {active.Level}";
        }
        else
        {
            companionNameText.text = "-";
            companionRoleText.text = "-";
            companionXPText.text = "XP: 0 / 0";
            companionLevelText.text = "Level: 0";
        }
    }

    public void SetDisplay(CompanionData companion)
    {
        if (companion == null) return;

        companionNameText.text = companion.GeneratedName;
        companionRoleText.text = companion.RoleName;
        companionXPText.text = $"XP: {companion.XP:F1} / {companion.XPToNextLevel:F1}";
        companionLevelText.text = $"Level: {companion.Level}";
    }

    public void ClearDisplay()
    {
        companionNameText.text = "-";
        companionRoleText.text = "-";
        companionXPText.text = "XP: 0 / 0";
        companionLevelText.text = "Level: 0";
    }

    void HandleLevelUp(CompanionData leveledCompanion)
    {
        if (leveledCompanion == activeCompanion)
            StartCoroutine(LevelUpFlash());
    }

    System.Collections.IEnumerator LevelUpFlash()
    {
        levelUpMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        levelUpMessage.gameObject.SetActive(false);
    }
}
