using UnityEngine;

namespace Scriptable_Objects
{
	[CreateAssetMenu(fileName = "New AI Agent Configuration", menuName = "Epidemic/Data/AI/Agent Config")]
	public class AIAgentConfigSO : ScriptableObject
	{
		public GameObject prefab;
		public ImpactSO impactSOData;
		public int LightDamage = 1;
		public int HeavyDamage = 2;
		public float AttackDistance = 1.4f;
		public int Experience = 10;
		public int coinDropAmount = 10;
		public float Speed = 2.5f;
		public int Health = 100;
	}
}
