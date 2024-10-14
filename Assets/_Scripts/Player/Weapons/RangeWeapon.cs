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
		public float Range => range;
		[SerializeField] protected WeaponDataSO weaponDataSO;
		[SerializeField] protected Transform gunBarrel;
		[SerializeField] protected ParticleSystem muzzleFlash;
		[SerializeField] protected float range=15;
		private PlayerAnimationManager playerAnimationManager;
		private ObjectPool<TrailRenderer> trailRendererPool;
		private EnemyDetector enemyDetector;
		private const float reload_time = 3;
		private float lastShotTime;
		private int bullets;
		private int damage;
		private int fireRate;
		private int magazineSize;

		public void InitializeWeapon(WeaponDataSO weaponData,EnemyDetector enemyDetector, PlayerAnimationManager playerAnimationManager)
		{
			this.playerAnimationManager = playerAnimationManager;
			trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
			damage = InitializeValues(ItemAttributeTypes.DAMAGE);
			magazineSize = InitializeValues(ItemAttributeTypes.MAGAZINE_SIZE);
			fireRate =InitializeValues(ItemAttributeTypes.FIRE_RATE);
			bullets = magazineSize;
			this.enemyDetector = enemyDetector;
		}

		private int InitializeValues(ItemAttributeTypes attributeType)
		{
			int level = SaveManager.Instance.GetPlayerData().playerTalents[weaponDataSO.itemID].talentLevels[attributeType];
			ItemAttribute attribute = weaponDataSO._itemAttributes[attributeType];
			return (int)(((attribute.maxValue - attribute.baseValue) / attribute.maxLevel) * level + attribute.baseValue);
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
				PerformReload(playerAnimationManager);
				return;
			}

			lastShotTime=(float)60/fireRate;
			ShotLogic();
		}

		private void ShotLogic()
		{
			if(enemyDetector.Size==0)
				return;

			IDamageAble damageable = enemyDetector.damageAbles[0];
			float distance = Vector3.Distance(damageable.Position, gunBarrel.transform.position);
			if(damageable.IsDead || distance>range)
				return;

			Vector3 normalizedDirection = Vector3.Normalize(damageable.Position - gunBarrel.transform.position);
			Vector3 forward = gunBarrel.transform.forward;
			if( distance>1.5f && Vector3.Dot(normalizedDirection, forward) < 0.5f)
				return;

			AudioManager.Instance.PlaySFX(SFXClips.AK47ShotSound);
			playerAnimationManager.PlayRifleMediumShot();
			bullets--;
			BulletCountChangedEventArgs bulletCountChangedEventArgs = new BulletCountChangedEventArgs
			{
				bulletCount = bullets,
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
			trailRenderer.transform.position =startPoint;
			trailRenderer.emitting = true;
			float distance = Vector3.Distance(startPoint, endPoint);
			float remainingDistance = distance;

			while (remainingDistance > 0)
			{
				trailRenderer.transform.position = Vector3.Lerp(startPoint, endPoint,
																Mathf.Clamp01(1 - (remainingDistance / distance)));
				remainingDistance -= weaponDataSO.TrailRenderer.SimulationSpeed * Time.deltaTime;

				yield return null;
			}
			trailRenderer.transform.position = endPoint;
			damageAble.HandleImpact(new ImpactData
			{
				HitPoint = endPoint,
				HitNormal = endPoint - startPoint,
				ImpactType = ImpactType.KNOCK_BACK,
			});
			yield return new WaitForSeconds(weaponDataSO.TrailRenderer.Duration);
			trailRenderer.emitting = false;
			trailRenderer.gameObject.SetActive(false);
			trailRendererPool.Release(trailRenderer);
		}

		private void PerformReload(PlayerAnimationManager playerAnimation)
		{
			lastShotTime = reload_time;
			bullets = magazineSize;
			playerAnimation.PlayReloadAnimation(weaponDataSO.reloadAnimation.name);
			AudioManager.Instance.PlaySFX(SFXClips.AK47ReloadSound);
		}

		private TrailRenderer CreateTrail()
		{
			GameObject instance = new GameObject("Bullet Trail");
			TrailRenderer trail = instance.AddComponent<TrailRenderer>();
			trail.colorGradient = weaponDataSO.TrailRenderer.Color;
			trail.material = weaponDataSO.TrailRenderer.Material;
			trail.widthCurve = weaponDataSO.TrailRenderer.widthCurve;
			trail.time = weaponDataSO.TrailRenderer.Duration;
			trail.minVertexDistance = weaponDataSO.TrailRenderer.MinVertexDistance;
			trail.emitting = false;
			trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			return trail;
		}
	}
}
