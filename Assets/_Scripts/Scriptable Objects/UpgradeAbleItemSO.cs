using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data
{
	[ CreateAssetMenu(fileName = "UpgradeAbleItem", menuName = "Data/UpgradeAbleItem")]
	public class UpgradeAbleItemSO:SerializedScriptableObject
	{
		public SerializedDictionary<ItemAttributeTypes,ItemAttribute> _itemAttributes=new SerializedDictionary<ItemAttributeTypes, ItemAttribute>();
		public bool isLocked;
		public int unlockPrice;
		public int unlockLevel;
		public Sprite backgroundIcon;
		public int itemID;
	}

	[Serializable]
	public class ItemAttribute
	{
		public Sprite icon;
		public string title;
		public int maxLevel;
		public float baseValue;
		public float maxValue;
	}
}
