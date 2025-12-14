using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Weapon weapon;

    void Update()
    {
        if (weapon != null)
        {
            weapon.Use(GetComponent<DiceCharacter>());
        }
    }
}
