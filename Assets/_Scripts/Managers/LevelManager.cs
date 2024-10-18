using Data;
using Event_System;
using Interfaces;
using Scriptable_Objects;
using UI.Managers;
using UnityEngine;

namespace Managers
{
	public class LevelManager : MonoBehaviour, ITimeDependent
	{
		[SerializeField]
		private UIInGameManager uiInGameManager;
		[SerializeField]
		private EnemyDatabaseSO enemyDatabaseSO;
		private LevelProgressData levelProgressData;
		private PlayerData playerData;

		private void Awake()
		{
			playerData = SaveManager.Instance.GetPlayerData();

			levelProgressData = new LevelProgressData
				(SaveManager.Instance.GetCurrentLevelData().levelDurationInSeconds,
				 0,
				 0,
				 playerData.currentExperience,
				 playerData.currentPlayerLevel, playerData.experienceToNextLevel * playerData.currentPlayerLevel, playerData.playerHealth);

			if (uiInGameManager == null)
			{
				uiInGameManager = FindObjectOfType<UIInGameManager>();
			}

			uiInGameManager.InitializeUI(levelProgressData);
		}

		private void OnEnable()
		{
			EventManager.Subscribe<EnemyDamagedEventArgs>(HandleEnemyKilled);
			EventManager.Subscribe<PlayerDamagedEventArgs>(HandlePlayerDamaged);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<EnemyDamagedEventArgs>(HandleEnemyKilled);
			EventManager.UnSubscribe<PlayerDamagedEventArgs>(HandlePlayerDamaged);
		}

		public void OnTimeUpdate(long currentTime)
		{
			if (levelProgressData.remainingTime < 0)
			{
				return;
			}

			levelProgressData.remainingTime--;
			uiInGameManager.UpdateUI(levelProgressData);
			if (levelProgressData.remainingTime != 0) return;

			LevelEndEventArgs levelEndEventArgs = new LevelEndEventArgs
			{
				isLevelCompleted = true
			};

			EventManager.RaiseEvent(levelEndEventArgs);
			SaveManager.Instance.SetPlayerData(playerData);
		}

		private void HandleEnemyKilled(EnemyDamagedEventArgs data)
		{
			if (!data.isDead)
			{
				return;
			}

			levelProgressData.killCount++;
			levelProgressData.coinAmount += enemyDatabaseSO.enemies[data.EnemyType].coinDropAmount;
			levelProgressData.experience += enemyDatabaseSO.enemies[data.EnemyType].Experience;

			if (levelProgressData.experience >= levelProgressData.experienceToNextLevel)
			{
				levelProgressData.currentLevel++;
				levelProgressData.experience -= levelProgressData.experienceToNextLevel;
				levelProgressData.experienceToNextLevel = playerData.experienceToNextLevel * playerData.currentPlayerLevel;
			}

			uiInGameManager.UpdateUI(levelProgressData);
		}

		private void HandlePlayerDamaged(PlayerDamagedEventArgs damageData)
		{
			levelProgressData.playerHealth -= damageData.Damage;
			uiInGameManager.UpdateUI(levelProgressData);
			if (levelProgressData.playerHealth > 0) return;

			LevelEndEventArgs levelEndEventArgs = new LevelEndEventArgs
			{
				isLevelCompleted = false
			};

			EventManager.RaiseEvent(levelEndEventArgs);
		}
	}
}
