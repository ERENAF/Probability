using UnityEngine;

public class Buffer : Item
{
    public int bonus;
    private void OnEnable()
    {
        itemname = "Квантовый стабилизатор";
        description = "Вы подчинаете вероятность лучше, при выпадении 1, переброситься результат";
        rarity = Rarity.Rare;
    }

    public override void OnEquip(DiceCharacter character)
    {
        bonus = DiceSystem.Roll(DiceType.D4,1);
        character.bonusDMG += bonus;
    }
    public override void OnUnequip(DiceCharacter character)
    {
        character.bonusDMG -= bonus;
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
