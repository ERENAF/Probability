using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] protected GameObject Item;
    [SerializeField] protected string tag_ = "Player";
    [SerializeField] protected string describtion;

    private void OnTriggerStay(Collider other)
    {
        DropOnTriggerStay(other);
    }


    protected virtual void DropOnTriggerStay(Collider other)
    {
        if (other.CompareTag(tag_))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.GetComponent<DicePlayer>().AddItem(Item);
                Destroy(gameObject);
            }
        }
    }
}
