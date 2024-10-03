using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.UI;
using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemAttributesView : MonoBehaviour
{
	[SerializeField] private Transform itemAttributeView;
	[SerializeField] private GameObject attributeHolderPrefab;
	[SerializeField] private GameObject attributeViewPrefab;
	[SerializeField] private GameObject UIAttributeUnlockButtonPrefab;
	[SerializeField] private Button upgradeButton;
	[SerializeField] private TextMeshProUGUI upgradeCountText;
	[SerializeField] private TextMeshProUGUI upgradeCostText;
	[SerializeField] WeaponDatabaseSO WeaponDatabaseSO;
	private readonly List<UIAttributeView> attributeViews = new List<UIAttributeView>();
	private bool isUpgrading;
	private const float upgrade_time = 2f;
	private WaitForSeconds upgradeWait;
	private PlayerData playerData;

	private void Awake()
	{
		upgradeWait = new WaitForSeconds(upgrade_time / 10);
		playerData = SaveManager.Instance.GetPlayerData();
		InitializeAttributeViews();
		UpdateUpgradeFields();
	}

	private void InitializeAttributeViews()
	{
		foreach (UpgradeAbleItemSO upgradeAbleItem in WeaponDatabaseSO.items.Values.Reverse())
		{
			GameObject attributeHolder = Instantiate(attributeHolderPrefab, itemAttributeView);
			InitializeAttributes(upgradeAbleItem, attributeHolder);
			CheckForLockedItems(upgradeAbleItem, attributeHolder);
		}
	}

	private void InitializeAttributes(UpgradeAbleItemSO upgradeAbleItem, GameObject attributeHolder)
	{
		foreach (Transform child in attributeHolder.transform)
		{
			Destroy(child.gameObject);
		}

		foreach (KeyValuePair<ItemAttributeTypes, ItemAttribute> attribute in upgradeAbleItem._itemAttributes)
		{
			UIAttributeView attributeView = Instantiate(attributeViewPrefab, attributeHolder.transform).GetComponent<UIAttributeView>();
			if (!playerData.playerTalents[upgradeAbleItem.itemID].isUnlocked) continue;
			attributeView.SetAttribute(attribute.Value, upgradeAbleItem.backgroundIcon);
			attributeView.itemID = upgradeAbleItem.itemID;
			attributeView.attributeType = attribute.Key;
			attributeViews.Add(attributeView);
			SetAttributeView(attributeView.itemID, attributeView.attributeType);
		}
	}

	private void CheckForLockedItems(UpgradeAbleItemSO upgradeAbleItem, GameObject attributeHolder)
	{
		if (playerData.playerTalents[upgradeAbleItem.itemID].isUnlocked) return;

		UIItemUnlockButton unlockButton = Instantiate(UIAttributeUnlockButtonPrefab, attributeHolder.transform).GetComponentInChildren<UIItemUnlockButton>();
		unlockButton.TalentNameText.text = "Unlock "+upgradeAbleItem.name;
		unlockButton.UnlockPriceText.text = "X"+upgradeAbleItem.unlockPrice;
		unlockButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
		  {
			  playerData.playerTalents[upgradeAbleItem.itemID].isUnlocked = true;
			  playerData.currentGold -= WeaponDatabaseSO.items[upgradeAbleItem.itemID].unlockPrice;
			  SaveManager.Instance.SetPlayerData(playerData);
			  unlockButton.RectTransform.gameObject.SetActive(false);
			  InitializeAttributes(upgradeAbleItem, attributeHolder);
		  });
		unlockButton.RectTransform.anchoredPosition = attributeHolder.GetComponent<RectTransform>().anchoredPosition;
	}

	private void OnEnable()
	{
		upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
	}

	private void OnDisable()
	{
		upgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
	}

	private void OnUpgradeButtonClick()
	{
		if (isUpgrading)
			return;
		StartCoroutine(RandomSelectionAnimation());
	}

	private IEnumerator RandomSelectionAnimation()
	{
		isUpgrading = true;
		playerData = SaveManager.Instance.GetPlayerData();
		float timer = 0;
		int randomIndex = Random.Range(0, attributeViews.Count);
		attributeViews[randomIndex].selectedHighlight.gameObject.SetActive(true);
		while(timer < upgrade_time)
		{
			yield return upgradeWait;
			attributeViews[randomIndex].selectedHighlight.gameObject.SetActive(false);

			int newRandomIndex = Random.Range(0, attributeViews.Count);
			while(newRandomIndex == randomIndex)
			{
				newRandomIndex = Random.Range(0, attributeViews.Count);
			}
			randomIndex = newRandomIndex;
			attributeViews[randomIndex].selectedHighlight.gameObject.SetActive(true);
			timer += upgrade_time/10;
		}
		yield return upgradeWait;
		attributeViews[randomIndex].selectedHighlight.gameObject.SetActive(false);
		int itemID = attributeViews[randomIndex].itemID;
		ItemAttributeTypes attributeType = attributeViews[randomIndex].attributeType;
		playerData.playerTalents[itemID].talentLevels[attributeType] += 1;
		playerData.currentGold -= playerData.upgradeCost;
		playerData.upgradeCount++;
		playerData.upgradeCost += 100;
		UpdateUpgradeFields();
		SaveManager.Instance.SetPlayerData(playerData);
		SetAttributeView(itemID, attributeType);
		isUpgrading = false;
	}

	private void UpdateUpgradeFields()
	{
		upgradeCostText.text = "X"+playerData.upgradeCost;
		upgradeCountText.text = "Upgraded "+playerData.upgradeCount+" times";
	}

	private void SetAttributeView(int itemID, ItemAttributeTypes attributeType)
	{
		UIAttributeView attributeView = attributeViews.Find(x => x.itemID == itemID && x.attributeType == attributeType);

		Debug.Log(itemID);
		foreach (var VARIABLE in playerData.playerTalents[itemID].talentLevels.Keys)
		{
			Debug.Log(VARIABLE);

		}
		int level = playerData.playerTalents[itemID].talentLevels[attributeType];
		attributeView.levelText.text = "Lv. " + level;
	}
}
