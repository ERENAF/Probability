using UnityEngine;

public class RoomLootSpawner : MonoBehaviour
{
    [Header("Будет ли появляться лут")]
    [SerializeField] private bool isLootSpawning;

    RoomData roomData;

    public bool IsLootSpawning => isLootSpawning;


    public void Init(RoomData roomData)
    {
        this.roomData = roomData;
    }

    public void Spawn()
    {
        if (!isLootSpawning) return;

        foreach (var item in roomData.LootSpawnPoints)
        {
            if (UnityEngine.Random.Range(0, 10) < roomData.LootSpawnChance) return;

            GameObject lootItem = Instantiate(roomData.LootListSO.LootList[UnityEngine.Random.Range(0, roomData.LootListSO.LootList.Count)]);

            if (lootItem == null) return;

            lootItem.transform.position = item.position + Vector3.up*1f;
        }
    }
}
