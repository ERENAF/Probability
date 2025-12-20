
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ShotGun : Weapon
{
    [Header("Настройки Дробаша")]
    public int pelletCount = 1;
    public float spreadAngle = 10f;
    private void OnEnable()
    {
        itemname = "Дробовик";

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

        Vector3 baseDirection = playerCamera.GetShootDirectionFromPoint(RifleStart.position);

        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 pelletDirection = GetPelletDirection(baseDirection, spreadAngle);

            GameObject pelletObj = Instantiate(
                BulletPrefab,
                RifleStart.position,
                Quaternion.LookRotation(pelletDirection)
            );


            pelletObj.GetComponent<Bullet>().Initialize(
                -Damage(dmgmodifier = character.StrengthMod + dmgmodifier,
        dmgmultipliyer),
                40 + character.IntelligenceMod,
                pelletDirection,
                RollType.RollSimple,
                character.IntelligenceMod + checkmodifier,
                character.reRoll
            );
        }
    }

    private Vector3 GetPelletDirection(Vector3 baseDirection, float maxAngle)
    {

        float randomAngle = Random.Range(0f, maxAngle);
        Vector3 randomAxis = Random.onUnitSphere;

        Quaternion randomRotation = Quaternion.AngleAxis(randomAngle, randomAxis);
        return randomRotation * baseDirection;
    }
}
