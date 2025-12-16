using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] protected GameObject Item;
    [SerializeField] protected string tag_ = "Player";
    [SerializeField] protected string describtion;
    [SerializeField] protected TextMeshProUGUI revealText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag_))
        {
            if (Item != null)
            {
                revealText.text = describtion;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        DropOnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (Item != null)
        {
            revealText.text = "";
        }
    }

    protected virtual void DropOnTriggerStay(Collider other)
    {
        if (other.CompareTag(tag_))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.GetComponent<DicePlayer>().AddItem(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
