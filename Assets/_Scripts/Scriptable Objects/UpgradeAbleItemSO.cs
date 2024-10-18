using Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scriptable_Objects
{
	[CreateAssetMenu(fileName = "UpgradeAbleItem", menuName = "Data/UpgradeAbleItem")]
	public class UpgradeAbleItemSO : SerializedScriptableObject
	{
		public SerializedDictionary<ItemAttributeTypes, ItemAttributeData> _itemAttributes = new SerializedDictionary<ItemAttributeTypes, ItemAttributeData>();
		public bool isLocked;
		public int unlockPrice;
		public int unlockLevel;
		public Sprite backgroundIcon;
		public int itemID;
	}
}
