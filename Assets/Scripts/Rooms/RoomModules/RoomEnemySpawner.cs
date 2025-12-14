using UnityEngine;

public class RoomEnemySpawner : MonoBehaviour
{
    [Header("Будут ли появляться монстры")]
    [SerializeField] private bool isLootSpawning;

    RoomData roomData;

    public bool IsLootSpawning => isLootSpawning;


    public void Init(RoomData roomData)
    {
        this.roomData = roomData;
    }
}
