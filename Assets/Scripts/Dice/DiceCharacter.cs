using UnityEngine;

public enum AbilityScore
{
    Strength,
    Dexterity,
    Constituion,
    Intelligence,
    Wisdom,
    Charisma
}


[RequireComponent(typeof(Health))]
public class DiceCharacter : MonoBehaviour
{
    [Header("��������������")]
    public int strength = 10;
    public int dexterity = 10;
    public int constituion = 10;
    public int intelligence = 10;
    public int wisdom = 10;
    public int charisma = 10;

    [Header("������ ���������")]
    public int armorClass = 10;
    public int proficiencyBonus = 0;

    public bool reRoll = false;

    public int StrengthMod => DiceSystem.GetAbilityModifier(strength);
    public int DexterityMod => DiceSystem.GetAbilityModifier(dexterity);
    public int ConstitutionMod => DiceSystem.GetAbilityModifier(constituion);
    public int IntelligenceMod => DiceSystem.GetAbilityModifier(intelligence);
    public int WisdomMod => DiceSystem.GetAbilityModifier(wisdom);
    public int CharismaMod => DiceSystem.GetAbilityModifier(charisma);

    public bool MakeAbilityCheck(AbilityScore ability, int difficultyClass = 10 )
    {
        int abilityModifier = GetAbilityModifier(ability);
        int roll = DiceSystem.Roll(DiceType.D20, 1, abilityModifier);

        bool success = roll >= difficultyClass;
        return success;
    }

    public int GetAbilityModifier(AbilityScore ability)
    {
        switch (ability)
        {
            case AbilityScore.Strength: return StrengthMod;
            case AbilityScore.Dexterity: return DexterityMod;
            case AbilityScore.Constituion: return ConstitutionMod;
            case AbilityScore.Intelligence: return IntelligenceMod;
            case AbilityScore.Wisdom: return WisdomMod;
            case AbilityScore.Charisma: return CharismaMod;
            default: return 0;
        }
    }
}
