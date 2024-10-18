using Data;
using Event_System;
using Scriptable_Objects;
using UnityEngine;
using Utility;

namespace Managers
{
	[DefaultExecutionOrder(-1000)]
	public class SaveManager : PersistentSingleton<SaveManager>
	{
		[SerializeField]
		private WeaponDatabaseSO weaponDatabase;
		private readonly PlayerDataChangedEventArgs playerDataChangedEventArgs = new PlayerDataChangedEventArgs();
		private readonly SettingsChangedEventArgs settingsChangedEventArgs = new SettingsChangedEventArgs();
		private LevelData currentLevelData;
		private int currentLevelID;
		private JsonDataHandler<LevelData> levelDataHandler;
		private string levelSavePath;
		private PlayerData playerData;
		private JsonDataHandler<PlayerData> playerDataHandler;
		private string playerSavePath;
		private SettingsData settingsData;
		private JsonDataHandler<SettingsData> settingsDataHandler;
		private string settingsSavePath;

		protected override void Awake()
		{
			base.Awake();
			settingsSavePath = Application.persistentDataPath + "/settings";
			playerSavePath = Application.persistentDataPath + "/player";
			levelSavePath = Application.persistentDataPath + "/level";
			settingsDataHandler = new JsonDataHandler<SettingsData>();
			playerDataHandler = new JsonDataHandler<PlayerData>();
			levelDataHandler = new JsonDataHandler<LevelData>();
			LoadData();
		}

		private void LoadData()
		{
			settingsData = settingsDataHandler.Load(settingsSavePath) ?? new SettingsData();
			playerData = playerDataHandler.Load(playerSavePath) ?? new PlayerData(weaponDatabase);
			currentLevelID = playerData.currentGameLevel;
			currentLevelData = levelDataHandler.Load(levelSavePath + currentLevelID) ?? new LevelData();
			SaveData();
		}

		private void SaveData()
		{
			settingsDataHandler.Save(settingsData, settingsSavePath);
			playerDataHandler.Save(playerData, playerSavePath);
			levelDataHandler.Save(currentLevelData, levelSavePath + currentLevelID);
		}

		public SettingsData GetSettingsData()
		{
			return settingsData;
		}

		public void SetSettingsData(SettingsData data)
		{
			settingsData = data;
			settingsDataHandler.Save(settingsData, settingsSavePath);
			settingsChangedEventArgs.settingsData = settingsData;
			EventManager.RaiseEvent(settingsChangedEventArgs);
		}

		public PlayerData GetPlayerData()
		{
			LoadData();
			return playerData;
		}

		public void SetPlayerData(PlayerData data)
		{
			playerData = data;
			playerDataHandler.Save(playerData, playerSavePath);
			playerDataChangedEventArgs.playerData = playerData;
			EventManager.RaiseEvent(playerDataChangedEventArgs);
		}

		public LevelData GetCurrentLevelData()
		{
			return currentLevelData;
		}
	}
}
