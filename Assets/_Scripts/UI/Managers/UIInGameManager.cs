using System;
using System.Collections.Generic;
using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameManager : MonoBehaviour,ITimeDependent
{
	[SerializeField] private Slider levelSlider;
	[SerializeField] private TextMeshProUGUI levelSliderText;
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private TextMeshProUGUI killCountText;
	[SerializeField] private TextMeshProUGUI remainingTimeText;
	[SerializeField] private TextMeshProUGUI coinAmountText;
	[SerializeField] private RectTransform healthBar;
	[SerializeField] private GameObject heartPrefab;
	[SerializeField] Button pauseButton;
	[SerializeField] GameObject pausePage;
	[SerializeField] GameObject winPage;
	[SerializeField] GameObject losePage;
	private readonly List<Image> playerHealth = new List<Image>();
	private int remainingTime;
	private int healthIndex = 0;

	private void Awake()
	{
		PlayerData playerData = SaveManager.Instance.GetPlayerData();
		InitializeHealthBar(playerData.playerHealth);
		InitializeSlider(playerData.experienceToNextLevel,playerData.currentExperience,playerData.currentPlayerLevel);
		InitializeLevelStatus();
	}

	private void OnEnable()
	{
		pauseButton.onClick.AddListener(OnPauseButtonClicked);
		EventManager.RegisterHandler<OnUpdateUI>(UpdateUI);
		EventManager.RegisterHandler<OnPlayerDamaged>(HandleHealthBar);
		EventManager.RegisterHandler<OnPlayerDeath>(OpenLosePage);
		EventManager.RegisterHandler<OnLevelCompleted>(OpenWinPage);
	}

	private void OnDisable()
	{
		pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
		EventManager.UnregisterHandler<OnUpdateUI>(UpdateUI);
		EventManager.UnregisterHandler<OnPlayerDamaged>(HandleHealthBar);
		EventManager.UnregisterHandler<OnPlayerDeath>(OpenLosePage);
		EventManager.UnregisterHandler<OnLevelCompleted>(OpenWinPage);
	}

	private void OpenWinPage(OnLevelCompleted obj)
	{
		winPage.SetActive(true);
	}

	private void OpenLosePage(OnPlayerDeath obj)
	{
		losePage.SetActive(true);
	}

	private void HandleHealthBar(OnPlayerDamaged obj)
	{
		for(int i = 0; i < obj.Damage; i++)
		{
			if(healthIndex >= playerHealth.Count)
				return;
			playerHealth[healthIndex].enabled = false;
			healthIndex++;
		}
	}

	private void UpdateUI(OnUpdateUI data)
	{
		killCountText.text = data.killCount.ToString();
		coinAmountText.text = data.coinAmount.ToString();
		InitializeSlider(data.experienceToNextLevel,data.experience,data.currentLevel);
	}

	private void InitializeLevelStatus()
	{
		coinAmountText.text = "0";
		killCountText.text = "0";
		remainingTime = SaveManager.Instance.GetCurrentLevelData().levelDurationInSeconds;
		TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
		remainingTimeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
	}

	private void InitializeHealthBar(int playerHealthAmount)
	{
		foreach (Transform child in healthBar)
		{
			Destroy(child.gameObject);
		}
		for (int i = 0; i < playerHealthAmount; i++)
		{
			playerHealth.Add(Instantiate(heartPrefab, healthBar).GetComponent<Image>());
		}
	}

	private void InitializeSlider(int experienceToNextLevel,int experience,int playerLevel)
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

	public void OnTimeUpdate(long currentTime)
	{
		remainingTime--;
		if(remainingTime < 0)
			return;
		TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
		remainingTimeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
	}
}
