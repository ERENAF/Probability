using UnityEngine;

public class Stabilizer : Item
{
    private void OnEnable()
    {
        itemname = "Квантовый стабилизатор";
        description = "Вы подчинаете вероятность лучше, при выпадении 1, переброситься результат";
        rarity = Rarity.Rare;
    }

    public override void OnEquip(DiceCharacter character)
    {
        character.reRoll = true;
    }
    public override void OnUnequip(DiceCharacter character)
    {
        character.reRoll = false;
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
