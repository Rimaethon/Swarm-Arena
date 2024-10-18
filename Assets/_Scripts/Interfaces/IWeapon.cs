using Player;
using Scriptable_Objects;

namespace Interfaces
{
	public interface IWeapon
	{
		float Range { get; }

		void TryGiveDamage();

		void InitializeWeapon(WeaponDataSO weaponData, EnemyDetector enemyDetector, PlayerAnimationManager playerAnimationManager);
	}
}
