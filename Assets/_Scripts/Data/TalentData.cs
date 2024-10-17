using System;
using System.Collections.Generic;
using Enums;

namespace Data
{
	[Serializable]
	public class TalentData
	{
		public bool isUnlocked;
		public Dictionary<ItemAttributeTypes, int> talentLevels;

		public TalentData()
		{
			talentLevels = new Dictionary<ItemAttributeTypes, int>();
		}
	}
}
