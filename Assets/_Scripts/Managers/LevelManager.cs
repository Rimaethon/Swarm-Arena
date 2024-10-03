using Data;
using UnityEngine;

namespace Managers
{
	public class LevelManager:MonoBehaviour,ITimeDependent
	{
		private int levelTime;
		private int killCount;
		private int coinAmount;
		private int experience;
		private int experienceToNextLevel;
		private readonly OnLevelCompleted onLevelCompleted = new OnLevelCompleted();
		private readonly OnUpdateUI onUpdateUI = new OnUpdateUI();
		private PlayerData playerData;

		private void Awake()
		{
			levelTime = SaveManager.Instance.GetCurrentLevelData().levelDurationInSeconds;
			playerData = SaveManager.Instance.GetPlayerData();
			experienceToNextLevel = playerData.experienceToNextLevel*playerData.currentPlayerLevel;
			experience=playerData.currentExperience;
		}

		private void OnEnable()
		{
			EventManager.RegisterHandler<OnEnemyKilled>(HandleEnemyKilled);
		}

		private void OnDisable()
		{
			EventManager.UnregisterHandler<OnEnemyKilled>(HandleEnemyKilled);
		}

		private void HandleEnemyKilled(OnEnemyKilled data)
		{
			killCount++;
			coinAmount += data.coinAmount;
			experience += data.expAmount;

			if(experience >= experienceToNextLevel)
			{
				playerData.currentPlayerLevel++;
				experience -= experienceToNextLevel;
				experienceToNextLevel = playerData.experienceToNextLevel*playerData.currentPlayerLevel;
			}
			onUpdateUI.killCount = killCount;
			onUpdateUI.coinAmount = coinAmount;
			onUpdateUI.experience = experience;
			onUpdateUI.currentLevel = playerData.currentPlayerLevel;
			onUpdateUI.experienceToNextLevel = experienceToNextLevel;
			EventManager.Send(onUpdateUI);
		}

		public void OnTimeUpdate(long currentTime)
		{
			if (levelTime < 0)
				return;
			levelTime--;
			if (levelTime != 0) return;
			EventManager.Send(onLevelCompleted);
			SaveManager.Instance.SetPlayerData(playerData);
		}
	}
}
