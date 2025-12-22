using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocationController : MonoBehaviour
{
    [SerializeField] private LocationGenerator locationGenerator;

    private List<GameObject> roomsList;

    public Transform GetPlayerSpawnPosition()
    {
        foreach (Transform item in locationGenerator?.Hub?.transform)
            foreach (Transform item2 in item)
                if (item2.CompareTag("SpawnPoint"))
                    return item2;

        return null;
    }

    public void CreateLocation()
    {
        roomsList = new();

        this.locationGenerator.Init(roomsList);

        locationGenerator.ResetInfo();
        locationGenerator.GenerateLocation();
    }
}
