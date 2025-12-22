using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomSettingsSO", menuName = "CustomTools/LootListSO")]
public class LootListSO : ScriptableObject
{
    [SerializeField] private List<GameObject> enemyList;
    public List<GameObject> EnemyList => enemyList;
}
