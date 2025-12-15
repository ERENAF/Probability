using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomData), typeof(RoomLootSpawner), typeof(RoomEnemySpawner))]
public class RoomBrain : MonoBehaviour
{
    RoomData roomData;
    RoomLootSpawner lootSpawner;
    RoomEnemySpawner enemySpawner;

    private void Awake()
    {
        roomData = GetComponent<RoomData>();
        lootSpawner = GetComponent<RoomLootSpawner>();
        enemySpawner = GetComponent<RoomEnemySpawner>();

        //соединения 
    }
}
