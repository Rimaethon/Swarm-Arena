using Enums;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
	public class UIAttributeView : MonoBehaviour
	{
		public ItemAttributeTypes attributeType;
		public Image icon;
		public Image background;
		public TextMeshProUGUI levelText;
		public TextMeshProUGUI titleText;
		public Image selectedHighlight;
		public int itemID;

		public void SetAttribute(ItemAttributeData attributeData, Sprite backgroundSprite)
		{
			icon.sprite = attributeData.icon;
			background.sprite = backgroundSprite;
			levelText.text = "Lv. 0";
			titleText.text = attributeData.title;
		}
	}
}
