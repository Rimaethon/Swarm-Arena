using System.Collections.Generic;
using Managers;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MeleeDamageCollider : MonoBehaviour
{
    private ImpactManager impactManager;
	private int damage;
    private Collider damageCollider;
    private readonly OnImpact onImpact=new OnImpact();

    void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.enabled = false;
        damageCollider.isTrigger = true;
        onImpact.HitNormal = Vector3.forward;
        onImpact.ImpactType = ImpactType.Shot;
    }

    public void EnableCollider(int damage)
    {
        this.damage = damage;
        onImpact.ImpactStrength = damage;
        damageCollider.enabled = true;
    }

    public void DisableCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.layer == transform.root.gameObject.layer)
        {
            return;
        }
        if (!other.transform.root.gameObject.TryGetComponent<IDamageAble>(out var component)) return;
        onImpact.HitObject = other.transform.root.gameObject;
        onImpact.HitPoint = other.transform.root.gameObject.transform.position + Vector3.up;
        EventManager.Send(onImpact);
        component.TakeDamage(damage, gameObject);

    }
}
