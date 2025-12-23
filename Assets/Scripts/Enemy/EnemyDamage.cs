using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Настройки")]
    public float damageCooldown = 1f;
    public DiceType damageDice = DiceType.D6;
    public string targetTag = "Player";

    private float nextDamageTime = 0f;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag) && Time.time >= nextDamageTime)
        {
            ApplyDamage(other);
            nextDamageTime = Time.time + damageCooldown;
        }
    }

    void ApplyDamage(Collider target)
    {
        DiceCharacter attacker = GetComponent<DiceCharacter>();
        DiceCharacter defender = target.GetComponent<DiceCharacter>();
        Health defenderHealth = target.GetComponent<Health>();

        if (attacker == null || defender == null || defenderHealth == null)
        {
            Debug.LogWarning("Не все компоненты найдены!");
            return;
        }

        // Урон: модификатор силы + бросок кубика
        int damage = attacker.StrengthMod + DiceSystem.Roll(damageDice);

        // Проверка попадания
        ResultType hitResult = DiceSystem.CheckD20(
            RollType.RollSimple,
            defender.armorClass,
            0,
            false
        );

        switch (hitResult)
        {
            case ResultType.CriticalSuccess:
                defenderHealth.ChangeHP(-damage * 2);
                Debug.Log($"КРИТ! Урон: {damage * 2}");
                break;

            case ResultType.Success:
                defenderHealth.ChangeHP(-damage);
                Debug.Log($"Попадание! Урон: {damage}");
                break;

            default:
                Debug.Log("Промах!");
                break;
        }
    }
}
