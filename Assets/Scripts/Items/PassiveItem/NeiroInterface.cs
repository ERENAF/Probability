using Unity.VisualScripting;
using UnityEngine;

public class NeiroInterface : Item
{
    [Header("Настройки предмета")]
    public int intelligenceBonus = 1;
    private void OnEnable()
    {
        itemname = "Нейроинтерфейс";
        description = "Помогает в стрельбе!";
        rarity = Rarity.Common;
    }

    public override void OnEquip(DiceCharacter character)
    {
        character.wisdom += intelligenceBonus;
    }
    public override void OnUnequip(DiceCharacter character)
    {
        character.wisdom -= intelligenceBonus;
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
