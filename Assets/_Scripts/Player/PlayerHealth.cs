using Data;
using Event_System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Player
{
	public class PlayerHealth :MonoBehaviour,IDamageAble
	{
		public bool IsDead => playerHealth<=0;
		public Vector3 Position => transform.position;
		private int playerHealth;

		private void Awake()
		{
			playerHealth = SaveManager.Instance.GetPlayerData().playerHealth;
		}

		public void TakeDamage(int damage)
		{
			if(IsDead)
				return;
			playerHealth -= damage;
			PlayerDamagedEventArgs playerDamagedEventArgs=new PlayerDamagedEventArgs
			{
				Damage = damage,
				isDead = IsDead
			};
			EventManager.RaiseEvent(playerDamagedEventArgs);
		}

		public void HandleImpact(ImpactData impactData)
		{
		}
	}
}
