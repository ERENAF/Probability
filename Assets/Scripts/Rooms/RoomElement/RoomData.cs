using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] private RoomType roomType;
    public RoomType RoomType => roomType;

    [SerializeField] private GameObject roomObject;
    public GameObject RoomObject => roomObject;

    public List<DoorData> Doors;

    [SerializeField] private List<Transform> lootSpawnPoints;
    public List<Transform> LootSpawnPoints => lootSpawnPoints;


    [SerializeField] private List<Transform> enemySpawnPoints;
    public List<Transform> EnemySpawnPoints => enemySpawnPoints;


    public IEnumerable<DoorData> FreeDoors()
    {
        List<DoorData> list = new();

        foreach (var d in Doors)
            if (!d.IsUsed)
                list.Add(d);

        return list;
    }

    public bool IsDoorUsed(DoorData door)
    {
        // Проверяем, что дверь существует и помечена как использованная
        return door != null && door.IsUsed;
    }

    public void MarkDoorAsUsed(DoorData door)
    {
        if (door != null)
        {
            door.IsUsed = true;
            UpdateDoorState(); // Обновляем визуальное состояние
        }
    }

    public void UpdateDoorState()
    {
        // Обновляем визуальное состояние дверей
        foreach (var door in Doors)
        {
            if (door != null)
            {
                // Здесь может быть логика скрытия/показа двери
                // Например: door.gameObject.SetActive(!door.IsUsed);
            }
        }
    }
}
