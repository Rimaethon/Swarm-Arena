using Enums;
using Interfaces;
using Scriptable_Objects;
using UnityEngine;

namespace Player.Weapons
{
	public class RadiantField : MonoBehaviour, IWeapon
	{
		public float Range => range;
		private int damage;
		private float range;
		private float cooldown;
		private float timer;
		private EnemyDetector enemyDetector;

		public void InitializeWeapon(WeaponDataSO weaponData,EnemyDetector enemyDetector, PlayerAnimationManager playerAnimationManager)
		{
			damage = (int)weaponData._itemAttributes[ItemAttributeTypes.DAMAGE].baseValue;
			range = weaponData._itemAttributes[ItemAttributeTypes.RANGE].baseValue;
			cooldown = weaponData._itemAttributes[ItemAttributeTypes.COOLDOWN].baseValue;
			transform.localScale = new Vector3(range*2, 0.01f, range*2);
			this.enemyDetector=enemyDetector;
		}

		public void TryGiveDamage()
		{
			timer -= Time.fixedDeltaTime;
			if(timer > 0)
				return;

			for(int i=0;i<enemyDetector.Size;i++)
			{
				if(Vector3.Distance(enemyDetector.damageAbles[i].Position,transform.position)>range)
					break;
				enemyDetector.damageAbles[i]?.TakeDamage(damage);
			}
			timer = cooldown;
		}
	}
}
