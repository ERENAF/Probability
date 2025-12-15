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
        currAmmo = Mathf.Max(currAmmo - deltaAmmo,0);

        Vector3 shootDirection = playerCamera.GetShootDirectionFromPoint(RifleStart.position);

        GameObject bulletObj = Instantiate(BulletPrefab,RifleStart.position, Quaternion.LookRotation(shootDirection));

        int damage = DiceSystem.Roll(dice,dicecount, character.StrengthMod );

        bulletObj.GetComponent<Bullet>().Initialize(-damage,50+character.IntelligenceMod,shootDirection,RollType.RollSimple,character.IntelligenceMod);
    }


}
