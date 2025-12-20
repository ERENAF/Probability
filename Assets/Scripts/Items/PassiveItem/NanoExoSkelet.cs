using System;
using UnityEngine;

public class NanoExoSkelet: Item
{
    [Header("настройки предмета")]
    public int strengthBonus = 1;

    private void OnEnable()
    {
        itemname = "Нано экзоскелет";
        description = "Усилите свои силовые возможности!";
        rarity = Rarity.Uncommon;
    }

    public override void OnEquip(DiceCharacter character)
    {
        character.strength += strengthBonus;
    }
    public override void OnUnequip(DiceCharacter character)
    {
        character.strength -= strengthBonus;
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
