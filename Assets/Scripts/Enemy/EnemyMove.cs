using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float speed;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += speed * MovementVector() * Time.deltaTime;
    }

    private Vector3 MovementVector()
    {
        return playerTransform.position - transform.position;
    }


}
