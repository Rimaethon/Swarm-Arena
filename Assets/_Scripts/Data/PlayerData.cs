using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[Serializable]
	public class PlayerData
	{
		public int currentPlayerLevel;
		public int currentExperience;
		public int experienceToNextLevel;
		public int currentGold;
		public int currentEnergy;
		public int maxEnergy;
		public int currentGems;
		public int lastEnergyRefillUnixTime;
		public int currentGameLevel;
		public int playerHealth;
		public int upgradeCost;
		public int upgradeCount;
		public Dictionary<int,TalentData> playerTalents;

		public PlayerData(WeaponDatabaseSO weaponDatabaseSO)
		{
			currentPlayerLevel = 1;
			currentExperience = 0;
			experienceToNextLevel = 100;
			currentGold = 6000;
			currentEnergy = 50;
			maxEnergy = 50;
			currentGems = 100;
			lastEnergyRefillUnixTime = 0;
			currentGameLevel = 1;
			playerHealth = 5;
			upgradeCost = 100;
			upgradeCount = 0;
			playerTalents = new Dictionary<int, TalentData>();


			foreach (KeyValuePair<int, WeaponDataSO> variable in weaponDatabaseSO.items)
			{
				TalentData talentData = new TalentData();
				if (!variable.Value.isLocked)
				{
					talentData.isUnlocked = true;
				}
				playerTalents.Add(variable.Key,talentData);
				foreach (KeyValuePair<ItemAttributeTypes, ItemAttribute> VARIABLE2 in variable.Value._itemAttributes)
				{
					playerTalents[variable.Key].talentLevels.Add(VARIABLE2.Key,0);
				}
			}

		}
	}

	[Serializable]
	public class TalentData
	{
		public bool isUnlocked;
		public Dictionary<ItemAttributeTypes, int> talentLevels;
		public TalentData()
		{
			talentLevels = new Dictionary<ItemAttributeTypes, int>();
		}
	}
}
