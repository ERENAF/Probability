using System.Collections.Generic;
using UnityEngine;

public class CollliderChecker : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();

    public int touchingCount => colliderList.Count;

    private void Awake()
    {
        colliderList.Clear();
    }

    public void ClearCollisions()
    {
        colliderList.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == gameObject) 
            return;

        if (!colliderList.Contains(collision.collider)) 
            colliderList.Add(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == gameObject) 
            return;

        if (colliderList.Contains(collision.collider)) 
            colliderList.Remove(collision.collider);
    }
}
