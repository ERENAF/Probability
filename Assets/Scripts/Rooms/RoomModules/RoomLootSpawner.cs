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
}
