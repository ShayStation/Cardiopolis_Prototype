using UnityEngine;

[System.Serializable]
public class CompanionData
{
    public string GeneratedName;
    public Sprite Portrait;
    public CompanionType Type;

    public int Level = 1;
    public float XP = 0f;
    public float XPToNextLevel;
    public float SessionXPGained { get; private set; } = 0f;

    public float MaxHealth;
    public float Stamina;
    public float Speed;
    public float RecoveryRate;
    public float Strength;

    public string RoleName => Type != null ? Type.TypeName : "Unknown";

    public CompanionData(CompanionType type)
    {
        Type = type;

        // Assign XP progression
        XPToNextLevel = type.BaseXPToNextLevel;

        // Random name
        if (type.NamePool != null && type.NamePool.Count > 0)
            GeneratedName = type.NamePool[Random.Range(0, type.NamePool.Count)];
        else
            GeneratedName = "Unnamed";

        // Random portrait
        if (type.PortraitPool != null && type.PortraitPool.Count > 0)
            Portrait = type.PortraitPool[Random.Range(0, type.PortraitPool.Count)];

        // Copy stats from type
        MaxHealth = type.MaxHealth;
        Stamina = type.Stamina;
        Speed = type.Speed;
        RecoveryRate = type.RecoveryRate;
    }

    public void AddXP(float amount)
    {
        XP += amount;

        if (WorkoutSessionManager.Instance != null && WorkoutSessionManager.Instance.IsWorkoutActive)
        {
            SessionXPGained += amount;
        }

        if (XP >= XPToNextLevel)
        {
            Level++;
            XP -= XPToNextLevel;
            XPToNextLevel *= 1.2f;
            WorkoutSessionManager.Instance?.NotifyCompanionLeveledUp();
        }
    }

    public void ResetSessionXP()
    {
        SessionXPGained = 0f;
    }

    public void Reset()
    {
        XP = 0f;
        Level = 1;
    }
}
