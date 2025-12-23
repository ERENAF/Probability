using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : Health
{

    protected override void Death()
    {
        Debug.Log("Сммерть");
        Cursor.visible = true;
        SceneManager.LoadScene("menu");
    }
}
