using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomSettingsSO", menuName = "CustomTools/EnemyListSO")]
public class EnemyListSO : ScriptableObject
{
    [SerializeField] private List<GameObject> enemyList;
    public List<GameObject> LootList => enemyList;
}
