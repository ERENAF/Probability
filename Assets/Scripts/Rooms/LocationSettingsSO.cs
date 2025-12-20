using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO объект для настройки локации
/// </summary>
[CreateAssetMenu(fileName = "RoomSettingsSO", menuName = "CustomTools/RoomSettingsSO")]
public class LocationSettingsSO : ScriptableObject
{
    [Header("Hub room's")]
    [SerializeField] private List<GameObject> HubRoomsList;
    [field: NonSerialized] public List<GameObject> hubRoomsList => HubRoomsList;
    [Header("Rooms with loot item's")]
    [SerializeField] private List<GameObject> LootRoomsList;
    [field:NonSerialized] public List<GameObject> lootRoomsList => LootRoomsList;

    [Header("Rooms with enemy units")]
    [SerializeField] private List<GameObject> EnemyRoomsList;
    [field: NonSerialized] public List<GameObject> enemyRoomsList => EnemyRoomsList;

    [Header("Rooms — Passages")]
    [SerializeField] private List<GameObject> PassageRoomsList;
    [field: NonSerialized] public List<GameObject> passageRoomsList => PassageRoomsList;

    [Header("Other rooms")]
    [SerializeField] private List<GameObject> OtherRoomsList;
    [field: NonSerialized] public List<GameObject> otherRoomsList => OtherRoomsList;

    [Header("Boss rooms")]
    [SerializeField] private List<GameObject> BossRoomsList;
    [field: NonSerialized] public List<GameObject> bossRoomsList => BossRoomsList;



    [Header("Count Settings")]
    [SerializeField] private int LootRoomsCount;
    [field: NonSerialized] public int lootRoomsCount => LootRoomsCount;

    [SerializeField] private int EnemyRoomsCount;
    [field: NonSerialized] public int enemyRoomsCount => EnemyRoomsCount;

    [SerializeField] private int PassageRoomsCount;
    [field: NonSerialized] public int passageRoomsCount => PassageRoomsCount;

    [SerializeField] private int OtherRoomsCount;
    [field: NonSerialized] public int otherRoomsCount => OtherRoomsCount;


}
