using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Death()
    {
        Debug.Log("Сммерть");
        Application.Quit();
    }
    protected override void IsAlive()
    {
        if (currHP <= 0)
        {
            Death();
        }
    }
}
