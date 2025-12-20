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
        return Time.time - timer >= 1 / SpeedAtack && !isReloading && currAmmo != 0;
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
        ChangeCurrAmmo();

        Vector3 shootDirection = playerCamera.GetShootDirectionFromPoint(RifleStart.position);

        GameObject bulletObj = Instantiate(BulletPrefab,RifleStart.position, Quaternion.LookRotation(shootDirection));

        bulletObj.GetComponent<Bullet>().Initialize(
        -Damage(dmgmodifier = character.StrengthMod + dmgmodifier,
        dmgmultipliyer),
        50+character.IntelligenceMod,
        shootDirection,
        RollType.RollSimple,
        character.IntelligenceMod + checkmodifier,
        character.reRoll);
    }


}
