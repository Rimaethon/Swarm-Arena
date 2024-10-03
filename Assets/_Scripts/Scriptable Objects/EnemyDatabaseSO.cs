using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[ CreateAssetMenu(fileName = "Enemy Database", menuName = "Data/Enemy Database")]
public class EnemyDatabaseSO : SerializedScriptableObject
{
    public Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();
}
