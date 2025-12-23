using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Android;
using Unity.Serialization;
using System.Collections;
public class MenuManager : MonoBehaviour
{
    [SerializeField] Animator anim;
    public string trigger;
    public float timer = 0f;

    public void GameScene(int num_scene)
    {
        anim.SetTrigger(trigger);
        StartCoroutine(SceneStarter(num_scene));
    }

    private IEnumerator SceneStarter(int num_scene)
    {
        yield return new WaitForSeconds(timer);
        PlayScene(num_scene);
    }
    public void PlayScene(int num_scene)
    {
        SceneManager.LoadScene(num_scene);
    }
    public void ExitFromTheGame()
    {
        Application.Quit();
    }
    public void ExitToMenu(int num_scene)
    {
        SceneManager.LoadScene(num_scene);
    }

    public void Resume(GameObject menu)
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Pause(GameObject menu)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
        menu.SetActive(true);
    }
    public void ActivateElement(GameObject element)
    {
        element.SetActive(true);
    }
    public void DeactivateElement(GameObject element)
    {
        element.SetActive(false);
    }
}
