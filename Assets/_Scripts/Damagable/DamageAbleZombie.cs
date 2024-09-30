using Managers;
using UnityEngine;

[RequireComponent(typeof(BaseAIAgent))]
public class DamageAbleZombie : DamageAbleCharacter
{
	private readonly OnEnemyKilled onEnemyKilled=new OnEnemyKilled();

	protected override void CalculateDamage(int damage)
	{
		base.CalculateDamage(damage);
		if (!_isDead) return;
		onEnemyKilled.coinAmount = _agent.Config.coinDropAmount;
		onEnemyKilled.expAmount = _agent.Config.Experience;
		onEnemyKilled.position = transform.position;
		EventManager.Send(onEnemyKilled);
	}
}
