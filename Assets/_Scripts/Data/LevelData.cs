using System.Collections.Generic;

namespace Data
{
	public class LevelData
	{
		public int levelDurationInSeconds;
		public List<int> mobIDs;

		public LevelData()
		{
			levelDurationInSeconds = 120;
			mobIDs = new List<int>
			{
				0
			};
		}
	}

}
