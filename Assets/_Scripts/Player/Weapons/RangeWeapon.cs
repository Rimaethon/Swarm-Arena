using System.Collections;
using _Scripts.Player.Weapons;
using Data;
using Managers;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(AudioSource))]
public class RangeWeapon : MonoBehaviour, IRangeWeapon, IWeapon
{
	[Header("Weapon options")]
	[SerializeField] protected WeaponDataSO weaponDataSO;
	[SerializeField] protected GameObject burrel;
	[SerializeField] protected ParticleSystem muzzleFlash;
	protected PlayerAnimationManager playerAnimationManager;
	protected ObjectPool<TrailRenderer> trailRendererPool;
	private PlayerData playerData;
	private readonly OnBulletCountChanged onBulletCountChanged = new OnBulletCountChanged();
	protected readonly OnImpact onImpact = new OnImpact();
	private readonly OnDamage onDamage = new OnDamage();
	protected int bullets;
	private int damage;
	private int fireRate;
	private int magazineSize;
	private float lastShotTime;

	protected virtual void Awake()
	{
		trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
		playerData= SaveManager.Instance.GetPlayerData();
		onImpact.ImpactType = ImpactType.Shot;
	}

	public void InitializeWeapon(WeaponDataSO weaponData)
	{
		damage = InitializeValues(ItemAttributeTypes.DAMAGE);
		magazineSize = InitializeValues(ItemAttributeTypes.MAGAZINE_SIZE);
		fireRate =InitializeValues(ItemAttributeTypes.FIRE_RATE);
		bullets = magazineSize;
		onDamage.Damage = damage;
	}

	private int InitializeValues(ItemAttributeTypes attributeType)
	{
		int level = playerData.playerTalents[weaponDataSO.itemID].talentLevels[attributeType];
		ItemAttribute attribute = weaponDataSO._itemAttributes[attributeType];
		return (int)(((attribute.maxValue - attribute.baseValue) / attribute.maxLevel) * level + attribute.baseValue);
	}

	public void Construct(PlayerAnimationManager playerAnimationManager)
	{
		this.playerAnimationManager = playerAnimationManager;
	}


	public void PerformShot()
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

	protected void ShotLogic()
	{
		Ray ray = new Ray(burrel.transform.position, burrel.transform.forward);
		bool raycast = Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Enemy"));
		if (!raycast) return;
		IDamageAble damageable = hit.collider.GetComponent<IDamageAble>();
		if (damageable is not { IsDead: false }) return;
		AudioManager.Instance.PlaySFX(SFXClips.AK47ShotSound);
		playerAnimationManager.PlayRifleMediumShot();
		bullets--;
		onBulletCountChanged.bulletCount= bullets;
		EventManager.Send(onBulletCountChanged);
		damageable.TakeDamage(damage, transform.root.gameObject);
		onDamage.Position = hit.point;
		EventManager.Send(onDamage);
		StartCoroutine(PlayTrail(burrel.transform.position, hit.point, hit));
	}

	protected virtual IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
	{
		muzzleFlash.Play();
		TrailRenderer instance = trailRendererPool.Get();
		instance.gameObject.SetActive(true);
		instance.transform.position = startPoint;
		yield return null;
		instance.emitting = true;
		float distance = Vector3.Distance(startPoint, endPoint);
		float remainingDistance = distance;

		while (remainingDistance > 0)
		{
			instance.transform.position = Vector3.Lerp(startPoint, endPoint,
				Mathf.Clamp01(1 - (remainingDistance / distance)));
			remainingDistance -= weaponDataSO.TrailRenderer.SimulationSpeed * Time.deltaTime;

			yield return null;
		}
		instance.transform.position = endPoint;
		if (hit.collider != null)
		{
			onImpact.HitObject = hit.transform.gameObject;
			onImpact.HitPoint = hit.point;
			onImpact.HitNormal = hit.normal;
			EventManager.Send(onImpact);
		}

		yield return new WaitForSeconds(weaponDataSO.TrailRenderer.Duration);
		yield return null;

		instance.emitting = false;
		instance.gameObject.SetActive(false);
		trailRendererPool.Release(instance);
	}

	public void PerformReload(PlayerAnimationManager playerAnimation)
	{
		lastShotTime = 3;
		bullets = magazineSize;
		playerAnimation.PlayReloadAnimation(weaponDataSO.reloadAnimation.name);
		AudioManager.Instance.PlaySFX(SFXClips.AK47ReloadSound);
	}

	protected virtual TrailRenderer CreateTrail()
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
