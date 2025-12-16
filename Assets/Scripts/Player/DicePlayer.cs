using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DicePlayer : DiceCharacter
{
    [SerializeField] List<GameObject> items;

    public void AddItem(GameObject item)
    {
        items.Add(item);
        GetComponent<Item>().OnEquip(this);
    }

    public void DropItem(GameObject item)
    {
        items.Remove(item);
        GetComponent<Item>().OnUnequip(this);
    }
}
