using UnityEngine;

public class LocationController : MonoBehaviour
{
    [SerializeField] private LocationGenerator locationGenerator;

    public void Awake()
    {
        StartNewGame();
    }

    public void Init()
    {

    }

    public void StartNewGame()
    {
        locationGenerator.GenerateLocation();
    }
}
