using AI;

namespace Interfaces
{
	public interface IAIMeleeAttack
	{
		public void PerformLightMeleeAttack(BaseAIAgent agent, int damage);

		public void PerformHeavyMeleeAttack(BaseAIAgent agent, int damage);
	}
}
