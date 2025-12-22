using UnityEngine;

public class RoomEnemySpawner : MonoBehaviour
{
    [Header("Будут ли появляться монстры")]
    [SerializeField] private bool isEnemySpawning;

    RoomData roomData;

    public bool IsLootSpawning => isEnemySpawning;


    public void Init(RoomData roomData)
    {
        this.roomData = roomData;
    }

    public void Spawn()
    {
        if (!isEnemySpawning) return;

        foreach (var item in roomData.EnemySpawnPoints)
        {
            if (UnityEngine.Random.Range(0, 10) < roomData.EnemySpawnChance) return;

            GameObject enemy = Instantiate(roomData.EnemyList.LootList[UnityEngine.Random.Range(0, roomData.EnemyList.LootList.Count)]);

            if (enemy == null) return;

            enemy.transform.parent = transform;

            enemy.transform.position = item.position + Vector3.up*1f ;
        }
    }
}
