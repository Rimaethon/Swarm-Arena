using System.Collections.Generic;
using Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scriptable_Objects
{
	[CreateAssetMenu(fileName = "Enemy Database", menuName = "Data/Enemy Database")]
	public class EnemyDatabaseSO : SerializedScriptableObject
	{
		public Dictionary<EnemyType, AIAgentConfigSO> enemies = new Dictionary<EnemyType, AIAgentConfigSO>();
	}
}
