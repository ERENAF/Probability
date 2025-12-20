using UnityEngine;

public class MatricShield : Item
{
    [Header("настройки предмета")]
    public int armorBonus = 1;

    private void OnEnable()
    {
        itemname = "Матричная броня";
        description = "Блестящая броня защитит вас от всего. +1 к КД";
        rarity = Rarity.Uncommon;
    }

    public override void OnEquip(DiceCharacter character)
    {
        character.armorClass += armorBonus;
    }
    public override void OnUnequip(DiceCharacter character)
    {
        character.armorClass -= armorBonus;
    }
    public override bool CanUse(DiceCharacter character)
    {
        return false;
    }

    public override void Use(DiceCharacter user, DiceCharacter target = null)
    {
        return;
    }
}
