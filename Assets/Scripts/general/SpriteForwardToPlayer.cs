using UnityEngine;
using System;
public class SpriteForwardToPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private Transform imageTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        imageTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 direction = playerTransform.position - imageTransform.position;
        direction.y = -90;
        imageTransform.rotation = Quaternion.LookRotation(direction);
    }
}
