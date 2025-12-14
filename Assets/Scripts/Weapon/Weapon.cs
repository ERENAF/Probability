using UnityEngine;

public abstract class Weapon : Item
{
    [Header("Настройки оружия")]
    public DiceType dice = DiceType.D3;
    public int count = 1;
    public float SpeedAtack = 1f;
    protected float timer = 0f;
    public GameObject BulletPrefab;
    public Transform RifleStart;

    public abstract void Shoot(DiceCharacter character);
}
