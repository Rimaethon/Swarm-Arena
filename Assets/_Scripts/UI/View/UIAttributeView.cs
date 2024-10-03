using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class UIAttributeView:MonoBehaviour
	{
		public ItemAttributeTypes attributeType;
		public int itemID;
		public Image icon;
		public Image background;
		public TextMeshProUGUI levelText;
		public TextMeshProUGUI titleText;
		public Image selectedHighlight;

		public void SetAttribute(ItemAttribute attribute,Sprite backgroundSprite)
		{
			icon.sprite = attribute.icon;
			background.sprite = backgroundSprite;
			levelText.text = "Lv. 0";
			titleText.text = attribute.title;
		}
	}
}
