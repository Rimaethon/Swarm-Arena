using System;
using UnityEngine;

namespace Scriptable_Objects
{
	[Serializable]
	public class ItemAttributeData
	{
		public Sprite icon;
		public string title;
		public int maxLevel;
		public float baseValue;
		public float maxValue;
	}
}
