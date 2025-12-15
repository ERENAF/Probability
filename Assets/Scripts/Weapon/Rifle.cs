using UnityEngine;

public class Rifle : Weapon
{
private void OnEnable()
    {
        itemname = "Автомат";

    }

    public override void OnEquip(DiceCharacter character)
    {
        timer = Time.time;
    }

    public override void OnUnequip(DiceCharacter character)
    {
        throw new System.NotImplementedException();
    }

    public override bool CanUse(DiceCharacter character)
    {
        return Time.time - timer >= 1 / SpeedAtack;
    }

    public override void Use(DiceCharacter user, DiceCharacter target = null)
    {
        if (Input.GetButton("Fire1") && CanUse(user))
        {
            Shoot(user);
            timer = Time.time;
        }
    }

    public override void Shoot(DiceCharacter character)
    {
        GameObject bulletObj = Instantiate(BulletPrefab, RifleStart.position, RifleStart.rotation);
        Vector3 shootDirection = RifleStart.forward;
        int damage = DiceSystem.Roll(dice, dicecount, character.StrengthMod);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Initialize(
                -damage,
                50 + character.IntelligenceMod, // скорость
                shootDirection,
                RollType.RollSimple,
                character.IntelligenceMod // модификатор атаки
            );
    }


}
