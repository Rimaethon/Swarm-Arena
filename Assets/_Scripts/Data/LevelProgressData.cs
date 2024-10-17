namespace Data
{
	public class LevelProgressData
	{
		public int coinAmount;
		public int currentLevel;
		public int experience;
		public int experienceToNextLevel;
		public int killCount;
		public int playerHealth;
		public int remainingTime;

		public LevelProgressData(int remainingTime, int killCount, int coinAmount, int experience, int currentLevel, int experienceToNextLevel, int playerHealth)
		{
			this.remainingTime = remainingTime;
			this.killCount = killCount;
			this.coinAmount = coinAmount;
			this.experience = experience;
			this.currentLevel = currentLevel;
			this.experienceToNextLevel = experienceToNextLevel;
			this.playerHealth = playerHealth;
		}
	}
}
