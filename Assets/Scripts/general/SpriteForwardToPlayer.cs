using UnityEngine;
using System;
public class SpriteForwardToPlayer : MonoBehaviour
{
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            return;
        }

        Vector3 direction = playerTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
