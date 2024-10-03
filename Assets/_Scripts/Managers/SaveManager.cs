using Data;
using Rimaethon.Scripts.Utility;
using UnityEngine;

namespace Managers
{
	[DefaultExecutionOrder(-1000)]
	public class SaveManager : PersistentSingleton<SaveManager>
	{
		[SerializeField] WeaponDatabaseSO weaponDatabase;
		private JsonDataHandler<SettingsData> settingsDataHandler;
		private JsonDataHandler<PlayerData> playerDataHandler;
		private JsonDataHandler<LevelData> levelDataHandler;
		private OnSettingsChanged onSettingsChanged = new OnSettingsChanged();
		private OnPlayerDataChanged onPlayerDataChanged = new OnPlayerDataChanged();

		private SettingsData settingsData;
		private PlayerData playerData;
		private LevelData currentLevelData;

		private string settingsSavePath;
		private string playerSavePath;
		private string levelSavePath;
		private int currentLevelID;

		protected override void Awake()
		{
			base.Awake();
			settingsSavePath= Application.persistentDataPath + "/settings";
			playerSavePath = Application.persistentDataPath + "/player";
			levelSavePath = Application.persistentDataPath + "/level";
			settingsDataHandler = new JsonDataHandler<SettingsData>();
			playerDataHandler = new JsonDataHandler<PlayerData>();
			levelDataHandler = new JsonDataHandler<LevelData>();
			LoadData();
		}

		private void LoadData()
		{
			settingsData =settingsDataHandler.Load(settingsSavePath) ?? new SettingsData();
			playerData = playerDataHandler.Load(playerSavePath) ?? new PlayerData(weaponDatabase);
			currentLevelID = playerData.currentGameLevel;
			currentLevelData =levelDataHandler.Load(levelSavePath + currentLevelID) ?? new LevelData();
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
			onSettingsChanged.settingsData = settingsData;
			EventManager.Send(onSettingsChanged);
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
			onPlayerDataChanged.playerData = playerData;
			EventManager.Send(onPlayerDataChanged);

		}

		public LevelData GetCurrentLevelData()
		{
			return currentLevelData;
		}
	}
}
