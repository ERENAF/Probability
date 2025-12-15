using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Weapon : Item
{
    [Header("Настройки оружия")]
    public float allAmmo;
    public float maxAmmoInWeapon;
    public float currAmmo;
    public float deltaAmmo;
    public DiceType dice = DiceType.D3;
    public int dicecount = 1;
    public float SpeedAtack = 1f;
    protected float timer = 0f;
    public GameObject BulletPrefab;
    public Transform RifleStart;

    public Vector3 deltaPos;
    public PlayerCameraController playerCamera;
    public abstract void Shoot(DiceCharacter character);

    public void DecreaseAmmo()
    {
        currAmmo-=deltaAmmo;
    }
    public void Reload()
    {
        if (currAmmo == maxAmmoInWeapon || allAmmo == 0)
        {
            return;
        }
        else if (currAmmo != maxAmmoInWeapon && Input.GetKeyDown(KeyCode.R))
        {
            allAmmo -= maxAmmoInWeapon-currAmmo;
            currAmmo = Mathf.Min(maxAmmoInWeapon,allAmmo);
        }
        else if (currAmmo == 0)
        {
            allAmmo -= maxAmmoInWeapon;
            currAmmo = Mathf.Min(maxAmmoInWeapon,allAmmo);
        }
    }

    public virtual string ToStringAmmo()
    {
        return $"{currAmmo}|{allAmmo}";
    }

    public void AttachToTransform(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = deltaPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
