using System;
using System.Collections.Generic;
using Data;
using Event_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Managers
{
	public class UIInGameManager : MonoBehaviour
	{
		[SerializeField]
		private Slider levelSlider;
		[SerializeField]
		private TextMeshProUGUI levelSliderText;
		[SerializeField]
		private TextMeshProUGUI levelText;
		[SerializeField]
		private TextMeshProUGUI killCountText;
		[SerializeField]
		private TextMeshProUGUI remainingTimeText;
		[SerializeField]
		private TextMeshProUGUI coinAmountText;
		[SerializeField]
		private RectTransform healthBar;
		[SerializeField]
		private GameObject heartPrefab;
		[SerializeField]
		private Button pauseButton;
		[SerializeField]
		private GameObject pausePage;
		[SerializeField]
		private GameObject winPage;
		[SerializeField]
		private GameObject losePage;
		private readonly List<Image> playerHealthViews = new List<Image>();
		private int disabledHealthViewCount;

		private void OnEnable()
		{
			pauseButton.onClick.AddListener(OnPauseButtonClicked);
			EventManager.Subscribe<LevelEndEventArgs>(HandleLevelEndUI);
		}

		private void OnDisable()
		{
			pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
			EventManager.UnSubscribe<LevelEndEventArgs>(HandleLevelEndUI);
		}

		public void InitializeUI(LevelProgressData data)
		{
			InitializeHealthBar(data.playerHealth);
			UpdateUI(data);
		}

		public void UpdateUI(LevelProgressData data)
		{
			killCountText.text = data.killCount.ToString();
			coinAmountText.text = data.coinAmount.ToString();
			UpdateSlider(data.experienceToNextLevel, data.experience, data.currentLevel);
			TimeSpan timeSpan = TimeSpan.FromSeconds(data.remainingTime);
			remainingTimeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
			UpdateHealthBar(data.playerHealth);
		}

		private void HandleLevelEndUI(LevelEndEventArgs data)
		{
			if (data.isLevelCompleted)
			{
				winPage.SetActive(true);
			}
			else
			{
				losePage.SetActive(true);
			}
		}

		private void UpdateHealthBar(int playerHealth)
		{
			if (playerHealthViews.Count - disabledHealthViewCount == playerHealth)
			{
				return;
			}

			for (int i = 0; i < playerHealth; i++)
			{
				if (disabledHealthViewCount >= playerHealthViews.Count)
				{
					return;
				}

				playerHealthViews[disabledHealthViewCount].enabled = false;
				disabledHealthViewCount++;
			}
		}

		private void InitializeHealthBar(int playerHealthAmount)
		{
			foreach (Transform child in healthBar)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < playerHealthAmount; i++)
			{
				playerHealthViews.Add(Instantiate(heartPrefab, healthBar).GetComponent<Image>());
			}
		}

		private void UpdateSlider(int experienceToNextLevel, int experience, int playerLevel)
		{
			levelSlider.maxValue = experienceToNextLevel;
			levelSlider.value = experience;
			levelSliderText.text = $"{experience}/{experienceToNextLevel}";
			levelText.text = playerLevel.ToString();
		}

		private void OnPauseButtonClicked()
		{
			Time.timeScale = 0;
			pausePage.SetActive(true);
		}
	}
}
