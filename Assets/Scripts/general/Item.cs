using System;
using UnityEngine;
using UnityEngine.InputSystem;

enum Rarity
{
    Common,
    Uncommon,
    Rare,
    VeryRare,
    Legendary,
    Artifact
}

public abstract class Item: ScriptableObject
{
    [Header("Основная инфа")]
    public string itemname;
    public string description;
    public GameObject icon;

    [Header("Механика")]
    public int charges = 0;
    public int maxCharges = 0;

    public abstract void OnEquip(DiceCharacter character);
    public abstract void OnUnequip(DiceCharacter character);
    public abstract void CanUse(DiceCharacter character);
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
}