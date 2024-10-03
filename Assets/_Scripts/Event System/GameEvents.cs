using Data;
using UnityEngine;

namespace Managers
{
	public class OnSettingsChanged:EventBase
	{
		public SettingsData settingsData;
	}

	public class OnPlayerDataChanged:EventBase
	{
		public PlayerData playerData;
	}

	public class OnEnemyKilled:EventBase
	{
		public int coinAmount;
		public int expAmount;
		public Vector3 position;
	}

	public class OnBulletCountChanged:EventBase
	{
		public int bulletCount;
	}

	public class OnImpact:EventBase
	{
		public GameObject HitObject;
		public Vector3 HitPoint;
		public Vector3 HitNormal;
		public ImpactType ImpactType;
		public float ImpactStrength;
	}

	public class OnDamage:EventBase
	{
		public float Damage;
		public Vector3 Position;
	}

	public class OnPlayerDamaged:EventBase
	{
		public int Damage;
	}

	public class OnPlayerDeath:EventBase
	{
	}

	public class OnLevelCompleted:EventBase
	{
	}

	public class OnUpdateUI:EventBase
	{
		public int killCount;
		public int coinAmount;
		public int experience;
		public int currentLevel;
		public int experienceToNextLevel;
	}
}
