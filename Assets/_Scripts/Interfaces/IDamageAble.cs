using Data;
using UnityEngine;

namespace Interfaces
{
	public interface IDamageAble
	{
		bool IsDead { get; }
		Vector3 Position { get; }
		void TakeDamage(int damage);
		void HandleImpact(ImpactData impactData);
	}
}
