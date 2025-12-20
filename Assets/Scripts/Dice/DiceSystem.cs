using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

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

public enum RollType
{
    RollSimple,
    RollWithAdvantage,
    RollWithDisadvantage
}
public enum ResultType
{
    CriticalSuccess,
    Success,
    Lose,
    CriticalLose
}
public static class DiceSystem
{
    public static ResultType CheckD20(RollType type, int difficulty,int modifier, bool isReroll = false)
    {
        int rollresult;
        switch (type)
        {
            case (RollType.RollSimple):
                rollresult = Roll(DiceType.D20,1,modifier, isReroll);
                break;
            case (RollType.RollWithAdvantage):
                rollresult = RollWithAdvantage(modifier,isReroll);
                break;
            case (RollType.RollWithDisadvantage):
                rollresult = RollWithDisadvantage(modifier,isReroll);
                break;
            default:
                rollresult = 1;
                break;
        }
        if (rollresult == 20 + modifier)
        {
            return ResultType.CriticalSuccess;
        }
        else if (rollresult == 1 + modifier)
        {
            return ResultType.CriticalLose;
        }
        else if (rollresult >= difficulty)
        {
            return ResultType.Success;
        }
        else
        {
            return ResultType.Lose;
        }

    }
    public static ResultType CheckD100(int difficulty, int modifier, bool isReroll)
    {
        int result = Roll(DiceType.D100,1,modifier, isReroll);
        if (result == 20 + modifier)
        {
            return ResultType.CriticalSuccess;
        }
        else if (result == 1 + modifier)
        {
            return ResultType.CriticalLose;
        }
        else if (result >= difficulty)
        {
            return ResultType.Success;
        }
        else
        {
            return ResultType.Lose;
        }
    }



    public static int Roll(DiceType dice, int count = 1, int modifier = 0, bool isReroll = false)
    {
        int total = 0;

        for (int i = 0; i < count; i++)
        {
            int result = Random.Range(1, (int)dice + 1);
            if (result == 1 && isReroll && (dice == DiceType.D20 || dice == DiceType.D100))
            {
                result = Random.Range(1, (int)dice + 1);
            }
            total += result;
        }
        return total + modifier;
    }
    public static int RollWithAdvantage(int modifier = 0, bool isReroll = false)
    {
        int roll1 = Roll(DiceType.D20,1,modifier, isReroll);
        int roll2 = Roll(DiceType.D20,1,modifier, isReroll);
        return Mathf.Max(roll1, roll2);
    }

    public static int RollWithDisadvantage(int modifier = 0, bool isReroll = false)
    {
        int roll1 = Roll(DiceType.D20,1,modifier, isReroll);
        int roll2 = Roll(DiceType.D20,1,modifier, isReroll);
        return Mathf.Min(roll1,roll2);
    }

    public static int GetAbilityModifier(int score)
    {
        return Mathf.FloorToInt((score - 10) / 2f);
    }
}
