using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionCardUI : MonoBehaviour
{
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI roleText;
    public TextMeshProUGUI statsText;
    public Button selectButton;

    public CompanionData Companion { get; private set; }

    private bool isSelected = false;

    public void Setup(CompanionData data)
    {
        Companion = data;

        // Update portrait
        if (Companion.Portrait != null)
            portraitImage.sprite = Companion.Portrait;

        // Update name, role, and stats
        nameText.text = Companion.GeneratedName;
        roleText.text = Companion.RoleName;
        statsText.text = $"Lvl {Companion.Level}\nHP {Companion.MaxHealth}\nStamina {Companion.Stamina}\nSpeed {Companion.Speed}\nStrength {Companion.Strength}\nRecovery {Companion.RecoveryRate}";

        // Button logic (start workout or whatever you want)
        selectButton.onClick.AddListener(() =>
        {
            if (WorkoutSessionManager.Instance == null)
            {
                Debug.LogError("WorkoutSessionManager.Instance is null! Make sure it exists in the scene.");
                return;
            }

            // Block selection during workout if the toggle is off
            if (!CompanionUIManager.AllowSelectionDuringWorkout && WorkoutSessionManager.Instance.IsWorkoutActive)
            {
                Debug.Log("Cannot select/unselect companion during workout.");
                return;
            }

            bool isNowSelected = WorkoutSessionManager.Instance.ToggleSelection(Companion);
            SetSelected(isNowSelected);
        });

        WorkoutSessionManager.Instance.RegisterCard(this);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        // Change button text
        selectButton.GetComponentInChildren<TextMeshProUGUI>().text = selected ? "Unselect" : "Select";

        // Highlight the card visually (e.g., background color)
        var bgImage = GetComponent<Image>();
        if (bgImage != null)
            bgImage.color = selected ? new Color(0.3f, 0.9f, 0.4f, 0.5f) : Color.white;
    }
}
