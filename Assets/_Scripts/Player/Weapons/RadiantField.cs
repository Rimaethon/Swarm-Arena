using _Scripts.Player.Weapons;
using Data;
using Managers;
using UnityEngine;

public class RadiantField : MonoBehaviour, IWeapon
{
	private int damage;
	private float range;
	private float cooldown;
	private float timer;
	private OnDamage onDamage = new OnDamage();

	public void InitializeWeapon(WeaponDataSO weaponData)
	{
		damage = (int)weaponData._itemAttributes[ItemAttributeTypes.DAMAGE].baseValue;
		range = weaponData._itemAttributes[ItemAttributeTypes.RANGE].baseValue;
		cooldown = weaponData._itemAttributes[ItemAttributeTypes.COOLDOWN].baseValue;
		transform.localScale = new Vector3(range, 0.01f, range);
		onDamage.Damage = damage;
	}

	private void FixedUpdate()
	{
		timer -= Time.fixedDeltaTime;
	}

	private void OnTriggerStay(Collider other)
	{
		if(timer > 0) return;
		IDamageAble damageable = other.GetComponent<IDamageAble>();
		damageable?.TakeDamage(damage, gameObject);
		onDamage.Position = other.transform.position;
		EventManager.Send(onDamage);
		timer = cooldown;
	}
}
