using UnityEngine;
using System;
using TMPro;

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
    [NonSerialized] public bool isReroll;
    [SerializeField] private GameObject damageTextPrefab;

    [Header("Настройки спауна текста урона")]
    [SerializeField] private float spawnRadius = 0.5f; // Радиус случайного смещения
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 1f, 0); // Базовое смещение вверх
    [SerializeField] private float textLifetime = 1.5f; // Время жизни текста

    private Rigidbody rb;
    private bool hasHit = false;

    public void Initialize(int DMG, float speed, Vector3 direction, RollType rt, int modifier, bool isReroll)
    {
        this.DMG = DMG;
        this.speed = speed;
        this.direction = direction.normalized;
        this.rolltype = rt;
        this.modifier = modifier;
        this.isReroll = isReroll;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            Debug.LogError("Rigidbody не найден на пуле!");
        }

        Destroy(gameObject, lifeTime);
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
                    modifier,
                    isReroll
                );

                Health enemyHealth = other.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    string damageText = "";
                    int damageAmount = 0;

                    if (result == ResultType.CriticalSuccess)
                    {
                        damageAmount = 2 * DMG;
                        enemyHealth.ChangeHP(damageAmount);
                        damageText = $"КРИТ! {damageAmount}";
                        Debug.Log($"Критическое попадание в {other.name}! Урон: {damageAmount}");
                    }
                    else if (result == ResultType.Success)
                    {
                        damageAmount = DMG;
                        enemyHealth.ChangeHP(damageAmount);
                        damageText = damageAmount.ToString();
                        Debug.Log($"Попадание в {other.name}! Урон: {damageAmount}");
                    }
                    else
                    {
                        damageText = "Промах";
                        Debug.Log($"Промах по {other.name}!");
                    }

                    SpawnDamageText(other.transform.position, damageText, result);
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

    private void SpawnDamageText(Vector3 hitPosition, string text, ResultType resultType)
    {
        if (damageTextPrefab == null)
        {
            Debug.LogWarning("Префаб текста урона не назначен!");
            return;
        }

        // 1. Генерируем случайное смещение в пределах радиуса
        Vector3 randomOffset = new Vector3(
            UnityEngine.Random.Range(-spawnRadius, spawnRadius),
            UnityEngine.Random.Range(0, spawnRadius * 0.5f),
            UnityEngine.Random.Range(-spawnRadius, spawnRadius)
        );

        // 2. Позиция спауна = позиция попадания + базовое смещение + случайное смещение
        Vector3 spawnPosition = hitPosition + spawnOffset + randomOffset;

        // 3. Создаем текст урона
        GameObject damageTextInstance = Instantiate(
            damageTextPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // 4. Настраиваем текст
        DamageText damageTextComponent = damageTextInstance.GetComponent<DamageText>();
        if (damageTextComponent != null && damageTextComponent.text != null)
        {
            damageTextComponent.text.text = text;

            // Меняем цвет в зависимости от типа попадания
            switch (resultType)
            {
                case ResultType.CriticalSuccess:
                    damageTextComponent.text.color = Color.yellow;
                    damageTextComponent.text.fontSize *= 1.2f; // Увеличиваем размер для крита
                    break;
                case ResultType.Success:
                    damageTextComponent.text.color = Color.white;
                    break;
                case ResultType.Lose:
                    damageTextComponent.text.color = Color.gray;
                    break;
                case ResultType.CriticalLose:
                    damageTextComponent.text.color = Color.red;
                    break;
            }

            // Настраиваем время жизни
            Destroy(damageTextInstance, textLifetime);
        }
        else
        {
            // Альтернатива: если компонент DamageText не найден, ищем TextMeshPro напрямую
            TextMeshPro tmpText = damageTextInstance.GetComponentInChildren<TextMeshPro>();
            if (tmpText != null)
            {
                tmpText.text = text;
                Destroy(damageTextInstance, textLifetime);
            }
        }

        // 5. Направляем текст к камере (billboard эффект)
        StartCoroutine(MakeTextFaceCamera(damageTextInstance));
    }

    private System.Collections.IEnumerator MakeTextFaceCamera(GameObject textObject)
    {
        Transform textTransform = textObject.transform;
        Camera mainCamera = Camera.main;

        while (textObject != null && mainCamera != null)
        {
            // Поворачиваем текст к камере
            textTransform.LookAt(textTransform.position + mainCamera.transform.rotation * Vector3.forward,
                               mainCamera.transform.rotation * Vector3.up);

            // Двигаем текст вверх для эффекта всплывания
            textTransform.position += Vector3.up * Time.deltaTime * 0.5f;

            // Плавное исчезновение
            TextMeshPro tmp = textObject.GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                Color color = tmp.color;
                color.a -= Time.deltaTime / textLifetime;
                tmp.color = color;
            }

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * 2f);
    }
}
