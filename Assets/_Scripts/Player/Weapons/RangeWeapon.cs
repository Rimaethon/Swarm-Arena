using System.Collections;
using Data;
using Enums;
using Event_System;
using Interfaces;
using Managers;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Pool;

namespace Player.Weapons
{
	[RequireComponent(typeof(AudioSource))]
	public class RangeWeapon : MonoBehaviour, IWeapon
	{
		private const float reload_time = 3;
		[SerializeField]
		protected WeaponDataSO weaponDataSO;
		[SerializeField]
		protected Transform gunBarrel;
		[SerializeField]
		protected ParticleSystem muzzleFlash;
		[SerializeField]
		protected float range = 15;
		private int bullets;
		private int damage;
		private EnemyDetector enemyDetector;
		private int fireRate;
		private float lastShotTime;
		private int magazineSize;
		private PlayerAnimationManager playerAnimationManager;
		private ObjectPool<TrailRenderer> trailRendererPool;
		public float Range => range;

		public void InitializeWeapon(WeaponDataSO weaponData, EnemyDetector enemyDetector, PlayerAnimationManager playerAnimationManager)
		{
			this.playerAnimationManager = playerAnimationManager;
			trailRendererPool = new ObjectPool<TrailRenderer>(weaponData.TrailRenderer.GetTrailRendererInstance);
			damage = InitializeValues(ItemAttributeTypes.DAMAGE);
			magazineSize = InitializeValues(ItemAttributeTypes.MAGAZINE_SIZE);
			fireRate = InitializeValues(ItemAttributeTypes.FIRE_RATE);
			bullets = magazineSize;
			this.enemyDetector = enemyDetector;
		}

		public void TryGiveDamage()
		{
			if (lastShotTime > 0)
			{
				lastShotTime -= Time.deltaTime;
				return;
			}

			if (bullets == 0)
			{
				PerformReload();
				return;
			}

			lastShotTime = (float) 60 / fireRate;
			ShotLogic();
		}

		private int InitializeValues(ItemAttributeTypes attributeType)
		{
			int level = SaveManager.Instance.GetPlayerData().playerTalents[weaponDataSO.itemID].talentLevels[attributeType];
			ItemAttributeData attributeData = weaponDataSO._itemAttributes[attributeType];
			return (int) ((attributeData.maxValue - attributeData.baseValue) / attributeData.maxLevel * level + attributeData.baseValue);
		}

		private void ShotLogic()
		{
			if (enemyDetector.Size == 0)
			{
				return;
			}

			IDamageAble damageable = enemyDetector.damageAbles[0];
			float distance = Vector3.Distance(damageable.Position, gunBarrel.transform.position);

			if (damageable.IsDead || distance > range)
			{
				return;
			}

			Vector3 normalizedDirection = Vector3.Normalize(damageable.Position - gunBarrel.transform.position);
			Vector3 forward = gunBarrel.transform.forward;

			if (distance > 1.5f && Vector3.Dot(normalizedDirection, forward) < 0.5f)
			{
				return;
			}

			AudioManager.Instance.PlaySFX(SFXClips.AK47ShotSound);
			playerAnimationManager.PlayRifleMediumShot();
			bullets--;

			BulletCountChangedEventArgs bulletCountChangedEventArgs = new BulletCountChangedEventArgs
			{
				bulletCount = bullets
			};

			EventManager.RaiseEvent(bulletCountChangedEventArgs);
			damageable.TakeDamage(damage);
			StartCoroutine(PlayTrail(damageable));
		}

		private IEnumerator PlayTrail(IDamageAble damageAble)
		{
			muzzleFlash.Play();
			Vector3 startPoint = gunBarrel.transform.position;
			Vector3 endPoint = damageAble.Position;
			TrailRenderer trailRenderer = trailRendererPool.Get();
			trailRenderer.gameObject.SetActive(true);
			trailRenderer.transform.position = startPoint;
			trailRenderer.emitting = true;
			float distance = Vector3.Distance(startPoint, endPoint);
			float remainingDistance = distance;

			while (remainingDistance > 0)
			{
				trailRenderer.transform.position = Vector3.Lerp(startPoint, endPoint,
																Mathf.Clamp01(1 - remainingDistance / distance));

				remainingDistance -= weaponDataSO.TrailRenderer.SimulationSpeed * Time.deltaTime;

				yield return null;
			}

			trailRenderer.transform.position = endPoint;

			damageAble.HandleImpact(new ImpactData
			{
				HitPoint = endPoint,
				HitNormal = endPoint - startPoint,
				ImpactType = ImpactType.KNOCK_BACK
			});

			yield return new WaitForSeconds(weaponDataSO.TrailRenderer.Duration);
			trailRenderer.emitting = false;
			trailRenderer.gameObject.SetActive(false);
			trailRendererPool.Release(trailRenderer);
		}

		private void PerformReload()
		{
			lastShotTime = reload_time;
			bullets = magazineSize;
			playerAnimationManager.PlayReloadAnimation();
			AudioManager.Instance.PlaySFX(SFXClips.AK47ReloadSound);
		}
	}
}
