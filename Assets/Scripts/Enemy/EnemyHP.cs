using Unity.VisualScripting;
using UnityEngine;

public class EnemyHP : Health
{
    protected override void Death()
    {
        Destroy(gameObject);
    }
}
