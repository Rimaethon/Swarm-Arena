using System;
using Data;
using Enums;
using UnityEngine;

namespace Event_System
{
	public class SettingsChangedEventArgs : EventArgs
	{
		public SettingsData settingsData;
	}

	public class PlayerDataChangedEventArgs : EventArgs
	{
		public PlayerData playerData;
	}

	public class BulletCountChangedEventArgs : EventArgs
	{
		public int bulletCount;
	}

	public class EnemyDamagedEventArgs : EventArgs
	{
		public int Damage;
		public EnemyType EnemyType;
		public bool isDead;
		public Vector3 Position;
	}

	public class PlayerDamagedEventArgs : EventArgs
	{
		public int Damage;
		public bool isDead;
	}

	public class LevelEndEventArgs : EventArgs
	{
		public bool isLevelCompleted;
	}
}
