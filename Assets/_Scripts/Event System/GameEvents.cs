using System;
using Data;
using Enums;
using UnityEngine;

namespace Event_System
{
	public class SettingsChangedEventArgs:EventArgs
	{
		public SettingsData settingsData;
	}

	public class PlayerDataChangedEventArgs:EventArgs
	{
		public PlayerData playerData;
	}

	public class BulletCountChangedEventArgs:EventArgs
	{
		public int bulletCount;
	}

	public class EnemyDamagedEventArgs:EventArgs
	{
		public EnemyType EnemyType;
		public Vector3 Position;
		public short Damage;
		public bool isDead;
	}

	public class PlayerDamagedEventArgs:EventArgs
	{
		public int Damage;
		public bool isDead;
	}

	public class LevelEndEventArgs:EventArgs
	{
		public bool isLevelCompleted;
	}
}
