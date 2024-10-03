using System.Collections.Generic;
using Managers;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
	public List<Impact> Impacts;

	private void OnEnable()
	{
		EventManager.RegisterHandler<OnImpact>(HandleImpact);
	}

	private void OnDisable()
	{
		EventManager.UnregisterHandler<OnImpact>(HandleImpact);
	}

	public void HandleImpact(OnImpact impactData)
	{
		Impact impact = Impacts.Find(i => impactData.HitObject.CompareTag(i.SurfaceTag) && i.ImpactType == impactData.ImpactType);
		if (impact != null)
		{
			PlayEffect(impactData.HitPoint, impactData.HitNormal,impact);
		}
	}

	private void PlayEffect(Vector3 hitPoint, Vector3 hitNormal, Impact impact)
	{
		ObjectPool particlePool = ObjectPool.CreateInstance(impact.EffectPrefab, 10);
		PoolAbleObject instance = particlePool.GetObject(hitPoint + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal));
		instance.transform.forward = hitNormal;
	}
}
