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
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = -90;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
