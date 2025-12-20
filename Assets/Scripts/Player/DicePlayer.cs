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
        switch (Item.GetComponent<Item>().useType)
        {
            case UseType.active :
                AddActiveItem(Item);
                break;
            case UseType.passive:
                AddPassiveItem(Item);
                break;
            default:
                break;
        }
    }
    private void AddPassiveItem(GameObject item)
    {
        items.Add(item);
        item.GetComponent<Item>().OnEquip(this);
    }

    private void AddActiveItem(GameObject item)
    {
        if (activeItem != null)
        {
            return;
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
