using System.Collections.Generic;
using Data;
using Enums;
using Interfaces;
using Managers;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour
	{

		public float HorizontalSpeed => transform.InverseTransformDirection(moveDirection).normalized.x;
		public float VerticalSpeed => transform.InverseTransformDirection(moveDirection).normalized.z;
		public Vector3 moveDirection;

		[SerializeField]
		private Rig aimRig;
		[SerializeField]
		private Transform aimTarget;
		[SerializeField]
		private Transform weaponHolder;
		[SerializeField]
		private WeaponDatabaseSO weaponDatabase;
		[SerializeField]
		private float aimSmoothTime;
		[SerializeField]
		private float movementSpeed;
		[SerializeField]
		private float rotationSpeed;

		private readonly List<IWeapon> weapons = new List<IWeapon>();
		private CharacterController characterController;
		private EnemyDetector enemyDetector;
		private PlayerAnimationManager playerAnimationManager;
		private PlayerData playerData;
		private PlayerHealth playerHealth;

		private bool hasTarget;
		private float largestRange;
		private float aimSmoothVelocity;
		private const float aim_rig_weight = 1f;

		private void Awake()
		{
			playerAnimationManager = GetComponent<PlayerAnimationManager>();
			playerHealth = GetComponent<PlayerHealth>();
			characterController = GetComponent<CharacterController>();
			playerData = SaveManager.Instance.GetPlayerData();
			enemyDetector = new EnemyDetector(transform, LayerMask.GetMask("Enemy"));
			InitializeWeapons();
			enemyDetector.radius = largestRange;
		}

		private void Update()
		{
			if (playerHealth.IsDead)
				return;

			enemyDetector.GetDamageAblesInLargestRange();
			HandlePlayerRotation();
			characterController.Move(moveDirection * (movementSpeed * Time.deltaTime) + new Vector3(0, Physics.gravity.y / 2, 0));
			if (!hasTarget)
				return;
			HandleWeaponShooting();
		}


		private void InitializeWeapons()
		{
			foreach (WeaponDataSO weaponData in weaponDatabase.items.Values)
			{
				if (!playerData.playerTalents[weaponData.itemID].isUnlocked)
				{
					continue;
				}

				IWeapon weapon = weaponData.weaponType == WeaponType.HANDGUN ?
									 Instantiate(weaponData.weaponPrefab, weaponHolder).GetComponent<IWeapon>() : Instantiate(weaponData.weaponPrefab, transform).GetComponent<IWeapon>();

				weapon.InitializeWeapon(weaponData, enemyDetector, playerAnimationManager);
				weapons.Add(weapon);
				largestRange = Mathf.Max(largestRange, weapon.Range);
			}
		}

		private void HandleWeaponShooting()
		{
			foreach (IWeapon weapon in weapons)
			{
				weapon.TryGiveDamage();
			}
		}

		private void HandlePlayerRotation()
		{
			hasTarget = enemyDetector.damageAbles[0] != null;
			Vector3 position = hasTarget ? enemyDetector.damageAbles[0].Position : transform.position;
			aimRig.weight = Mathf.SmoothDamp(aimRig.weight, aim_rig_weight, ref aimSmoothVelocity, aimSmoothTime);
			Vector3 lookDirection = moveDirection;

			if (hasTarget)
			{
				lookDirection = position - transform.position;
				lookDirection.y = 0;
			}
			aimTarget.position = position + transform.forward * 6 + Vector3.up;

			if (lookDirection == Vector3.zero)
				return;

			Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, largestRange);
		}
	}
}
