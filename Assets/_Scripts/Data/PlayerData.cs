using System;
using System.Collections.Generic;
using Enums;
using Scriptable_Objects;

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
		public Dictionary<int, TalentData> playerTalents;

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

			foreach (KeyValuePair<int, WeaponDataSO> weaponData in weaponDatabaseSO.items)
			{
				TalentData talentData = new TalentData();

				if (!weaponData.Value.isLocked)
				{
					talentData.isUnlocked = true;
				}

				playerTalents.Add(weaponData.Key, talentData);

				foreach (KeyValuePair<ItemAttributeTypes, ItemAttributeData> attributeType in weaponData.Value._itemAttributes)
				{
					playerTalents[weaponData.Key].talentLevels.Add(attributeType.Key, 0);
				}
			}
		}
	}
}
