using UnityEngine;

public static class CompanionGenerator
{
    public static CompanionData Generate(CompanionType type)
    {
        return new CompanionData(type);
    }
}
