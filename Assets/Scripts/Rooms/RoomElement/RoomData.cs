using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] private GameObject roomObject;

    [SerializeField] private List<Transform> roomDoorsPoints;

    [SerializeField] private List<Transform> lootSpawnPoints;

    [SerializeField] private List<Transform> enemySpawnPoints;
}
