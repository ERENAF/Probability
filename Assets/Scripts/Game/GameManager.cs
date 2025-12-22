using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LocationController locationController;

    [SerializeField] PlayerSetuper playerLocationManager;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        locationController.CreateLocation();

        playerLocationManager.CreatePlayer(locationController.GetPlayerSpawnPosition());
    }
}
