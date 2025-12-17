using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    VeryRare,
    Legendary,
    Artifact
}

public enum UseType
{
    active,
    passive,
    weapon
}

public abstract class Item: MonoBehaviour
{
    [Header("�������� ����")]
    public string itemname;
    public string description;
    public Rarity rarity = Rarity.Common;
    public UseType useType = UseType.passive;
    public GameObject icon;

    [Header("��������")]
    public int charges = 0;
    public int maxCharges = 0;

    public abstract void OnEquip(DiceCharacter character);
    public abstract void OnUnequip(DiceCharacter character);
    public abstract bool CanUse(DiceCharacter character);
    public abstract void Use(DiceCharacter user, DiceCharacter target = null);

    public virtual void OnAttackRoll(DiceCharacter attacker, ref int rollResult) { }
    public virtual void OnDamageRoll(DiceCharacter attacker, ref int damage, string damageType) { }
    public virtual void OnSavingThrow(DiceCharacter character, AbilityScore ability, ref int rollResult) { }
    public virtual void OnAbilityCheck(DiceCharacter character, AbilityScore ability, ref int rollResult) { }

    protected string ApplyDamage(GameObject target, int damage, DiceCharacter source)
    {
        int actualDamage = Mathf.Max(0, damage);
        target.GetComponent<Health>().currHP-=actualDamage;
        return $"{actualDamage}";
    }

    protected virtual void DropItem(Transform transform)
    {
        Instantiate(icon, transform);
        Destroy(gameObject);
    }
    public virtual void DropItem(Transform transform, int Force)
    {
        GameObject obj = Instantiate(icon,transform.position,transform.rotation);
        float x = (float)Random.Range(0,100)/100;
        float z = (float)Random.Range(0,100)/100;

        obj.GetComponent<Rigidbody>().AddForce(new Vector3(x,transform.position.y,z).normalized,ForceMode.Impulse);
        Destroy(gameObject);
    }
}
