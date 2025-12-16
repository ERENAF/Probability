using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Weapon : Item
{
    [Header("Настройки оружия")]
    public float allAmmo;
    public float maxAmmoInWeapon;
    public float currAmmo;
    public float deltaAmmo;
    public float reloadTime = 1f;
    protected bool isReloading = false;
    public DiceType dice = DiceType.D3;
    public int dicecount = 1;
    public float SpeedAtack = 1f;
    protected float timer = 0f;
    public GameObject BulletPrefab;
    public Transform RifleStart;

    public Vector3 deltaPos;
    public PlayerCameraController playerCamera;
    public abstract void Shoot(DiceCharacter character);

    void Start()
    {
        currAmmo = maxAmmoInWeapon;
    }
    public void Reload(float timer )
    {
        if (currAmmo == maxAmmoInWeapon || allAmmo == 0)
        {
            return;
        }
        else if (currAmmo != maxAmmoInWeapon && Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            StartCoroutine(ReloadFunc(timer));
        }
        else if (currAmmo == 0)
        {
            isReloading = true;
            StartCoroutine(ReloadFunc(timer));
        }
    }
    public void Reload()
    {
        if (currAmmo == maxAmmoInWeapon || allAmmo == 0)
        {
            return;
        }
        else if ((currAmmo < maxAmmoInWeapon && Input.GetKeyDown(KeyCode.R)) || currAmmo == 0)
        {
            isReloading = true;
            StartCoroutine(ReloadFunc(reloadTime));
        }
    }
    private IEnumerator ReloadFunc(float timer = 0f)
    {
        yield return new WaitForSeconds(timer);
        if(allAmmo > maxAmmoInWeapon-currAmmo)
        {
            allAmmo -= maxAmmoInWeapon-currAmmo;
            currAmmo = maxAmmoInWeapon;
        }
        else if (allAmmo == maxAmmoInWeapon - currAmmo)
        {
            allAmmo = 0;
            currAmmo = maxAmmoInWeapon - currAmmo;
        }
        else if (allAmmo < maxAmmoInWeapon - currAmmo)
        {
            currAmmo += allAmmo;
            allAmmo = 0;
        }
        isReloading = false;
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
    public void ChangeCurrAmmo()
    {
        currAmmo = Mathf.Max(currAmmo - deltaAmmo,0);
    }
}
