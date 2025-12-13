using UnityEngine;
using System.Collections.Generic;

public enum DiceType
{
    D3 = 3,
    D4 = 4,
    D6 = 6,
    D8 = 8,
    D10 = 10,
    D12 = 12,
    D20 = 20,
    D100 = 100
}
public static class DiceSystem
{
    public static int Roll(DiceType dice, int count = 1, int modifier = 0)
    {
        int total = 0;
        for (int i = 0; i < count; i++)
        {
            total += Random.Range(1, (int)dice + 1);
        }
        return total + modifier;
    }
    public static int RollWithAdvantage()
    {
        int roll1 = Roll(DiceType.D20);
        int roll2 = Roll(DiceType.D20);
        return Mathf.Max(roll1, roll2);
    }

    public static int RollWithDisadvantage()
    {
        int roll1 = Roll(DiceType.D20);
        int roll2 = Roll(DiceType.D20);
        return Mathf.Min(roll1,roll2);
    }

    public static int GetAbilityModifier(int score)
    {
        return Mathf.FloorToInt((score - 10) / 2f);
    }
}
