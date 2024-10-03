using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceBarView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI energyAmountText;
    [SerializeField] private TextMeshProUGUI goldAmountText;
    [SerializeField] private TextMeshProUGUI gemAmountText;
    private PlayerData playerData;

    private void Awake()
    {
        playerData = SaveManager.Instance.GetPlayerData();
        UpdateResourceBars();
    }

    private void OnEnable()
    {
        EventManager.RegisterHandler<OnPlayerDataChanged>(HandlePlayerDataChanged);
    }

    private void OnDisable()
    {
        EventManager.UnregisterHandler<OnPlayerDataChanged>(HandlePlayerDataChanged);
    }

    private void HandlePlayerDataChanged(OnPlayerDataChanged data)
    {
        playerData = data.playerData;
        UpdateResourceBars();
    }

    private void UpdateResourceBars()
    {
        levelText.text =playerData.currentPlayerLevel.ToString();
        expSlider.value = (float)playerData.currentExperience / playerData.experienceToNextLevel*playerData.currentPlayerLevel;
        energyAmountText.text = playerData.currentEnergy+"/"+playerData.maxEnergy;
        goldAmountText.text = playerData.currentGold.ToString();
        gemAmountText.text = playerData.currentGems.ToString();
    }
}
