using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [NonSerialized] public int DMG;
    public string EnemyTag = "Enemy";
    public float lifeTime = 2f;
    [NonSerialized] public float speed = 1f;
    [NonSerialized] public Vector3 direction;
    [NonSerialized] public RollType rolltype;
    [NonSerialized] public int modifier;

    private Rigidbody rb;
    private bool hasHit = false;

    public void Initialize(int DMG, float speed, Vector3 direction, RollType rt, int modifier)
    {
        this.DMG = DMG;
        this.speed = speed;
        this.direction = direction.normalized;
        this.rolltype = rt;
        this.modifier = modifier;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Настройки Rigidbody для пули
        if (rb != null)
        {
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // Используем linearVelocity вместо velocity
            rb.linearVelocity = direction * speed;
        }
        else
        {
            Debug.LogError("Rigidbody не найден на пуле!");
        }

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Поддерживаем постоянную скорость
        if (rb != null && !hasHit)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        Debug.Log($"Пуля столкнулась с: {other.name}, тег: {other.tag}");

        if (other.CompareTag(EnemyTag))
        {
            hasHit = true;

            DiceCharacter enemyChar = other.GetComponent<DiceCharacter>();
            if (enemyChar != null)
            {
                ResultType result = DiceSystem.CheckD20(
                    rolltype,
                    enemyChar.armorClass,
                    modifier
                );

                Health enemyHealth = other.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    if (result == ResultType.CriticalSuccess)
                    {
                        enemyHealth.ChangeHP(2 * DMG);
                        Debug.Log($"Критическое попадание в {other.name}! Урон: {2 * DMG}");
                    }
                    else if (result == ResultType.Success)
                    {
                        enemyHealth.ChangeHP(DMG);
                        Debug.Log($"Попадание в {other.name}! Урон: {DMG}");
                    }
                    else
                    {
                        Debug.Log($"Промах по {other.name}!");
                    }
                }
                else
                {
                    Debug.LogError($"У объекта {other.name} нет компонента Health!");
                }
            }
            else
            {
                Debug.LogError($"У объекта {other.name} нет компонента DiceCharacter!");
            }

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Bullet") && !other.CompareTag("Weapon"))
        {
            Destroy(gameObject);
            Debug.Log($"Пуля уничтожена при столкновении с {other.name}");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * 2f);
    }
}
