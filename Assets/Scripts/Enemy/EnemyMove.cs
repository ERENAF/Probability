using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distance = 0;

    const float timer = 0.1f;
    private bool isActive;
    private Transform playerTransform;

    WaitForSeconds waitForSeconds = new WaitForSeconds(timer);
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // debilnoe reshenie no chto podelat mi ne v pro gamedev corp and job for truueee leeee
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            return;
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < distance * 10 && !isActive)
        {
            isActive = true;
        }

        if (Vector3.Distance(transform.position, playerTransform.position) > distance/3
            && Vector3.Distance(transform.position, playerTransform.position) < distance * 3
            && isActive)
        {
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        transform.position +=  MovementVector() * speed * Time.deltaTime;

        yield return waitForSeconds;
    }

    private Vector3 MovementVector()
    {
        return (playerTransform.position - transform.position).normalized;
    }
}
