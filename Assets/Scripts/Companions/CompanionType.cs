using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCompanionType", menuName = "Cardiopolis/Companion Type")]
public class CompanionType : ScriptableObject
{
    [Header("Identity")]
    public string TypeName; // e.g., "Hunter", "Gatherer"
    public List<Sprite> PortraitPool;
    public List<string> NamePool;

    [Header("XP")]
    public float BaseXPToNextLevel = 1000f;

    [Header("Base Stats")]
    public float MaxHealth = 100f;
    public float Stamina = 50f;
    public float Speed = 10f;
    public float RecoveryRate = 5f;
    public float Strength = 10f;
}
