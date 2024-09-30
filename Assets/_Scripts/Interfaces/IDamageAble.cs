using UnityEngine;

public interface IDamageAble
{
    public bool IsDead { get; }
    public void TakeDamage(int damage, GameObject targetCausedDamage);

}
