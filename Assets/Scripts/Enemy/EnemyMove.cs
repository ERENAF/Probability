using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float speed;
    public float distance = 0;
    public float timer;
    private bool isActive;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (MovementVector().magnitude < distance * 10 && !isActive)
        {
            isActive = true;
        }

        if (MovementVector().magnitude > distance && isActive)
        {
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        transform.position += speed * MovementVector().normalized * Time.deltaTime;
        yield return new WaitForSeconds(timer);
    }

    private Vector3 MovementVector()
    {
        return playerTransform.position - transform.position;
    }


}
