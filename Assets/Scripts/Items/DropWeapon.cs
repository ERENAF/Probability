using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DropWeapon : DropItem
{
    protected override void DropOnTriggerStay(Collider other)
    {
        if (other.CompareTag(tag_))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FindFirstObjectByType<PlayerShoot>().SetWeapon(Item);
                Destroy(gameObject);
            }
        }
    }
}
