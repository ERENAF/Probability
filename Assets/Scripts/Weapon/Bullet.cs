using UnityEngine;
using System;
using System.Data;
public class Bullet : MonoBehaviour
{
    [NonSerialized] public int DMG;
    public string EnemyTag = "Enemy";
    public float lifeTime = 2f;
    [NonSerialized] public float speed = 1f;
    [NonSerialized] public Vector3 direction;
    [NonSerialized] public RollType rolltype;
    [NonSerialized] public int modifier;

    public void Initialize(int DMG, float speed, Vector3 direction, RollType rt, int modifier)
    {
        this.DMG = DMG;
        this.speed = speed;
        this.direction = direction;
        this.rolltype = rt;
        this.modifier = modifier;
        Destroy(gameObject,lifeTime);
    }



    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(EnemyTag))
        {

            DiceCharacter enemyChar = other.GetComponent<DiceCharacter>();

            ResultType result = DiceSystem.CheckD20(
                rolltype,
                enemyChar.armorClass,
                modifier
            );

            Health enemyHealth = other.GetComponent<Health>();
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
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Bullet") && !other.CompareTag("Weapon"))
        {
            Destroy(gameObject);
            Debug.Log($"Пуля уничтожена при столкновении с {other.name}");
        }
    }
}
