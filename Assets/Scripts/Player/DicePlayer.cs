using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class DicePlayer : DiceCharacter
{
    [SerializeField] List<GameObject> items;
    [SerializeField] GameObject activeItem;

    public void AddItem(GameObject Item)
    {
        return;
    }
    public void AddPassiveItem(GameObject item)
    {
        items.Add(item);
        GetComponent<Item>().OnEquip(this);
    }

    public void AddActiveItem(GameObject item)
    {
        if (activeItem != null)
        {

        }
    }

    public void DropActiveItem()
    {
        if (activeItem != null)
        {
            return;

        }
    }
    public void DropPassiveItem(GameObject item)
    {
        items.FindLast(i => i == item);
        items.Remove(item);

    }
}
