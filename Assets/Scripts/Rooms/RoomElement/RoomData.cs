using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] private GameObject roomObject;
    public GameObject RoomObject => roomObject;

    [SerializeField] private List<Transform> roomDoorsPoints;
    public List<Transform> RoomDoorsPoints => roomDoorsPoints;


    [SerializeField] private List<Transform> lootSpawnPoints;
    public List<Transform> LootSpawnPoints => lootSpawnPoints;


    [SerializeField] private List<Transform> enemySpawnPoints;
    public List<Transform> EnemySpawnPoints => enemySpawnPoints;
}
