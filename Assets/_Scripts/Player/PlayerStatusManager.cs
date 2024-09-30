using Data;
using Managers;
using UnityEngine;


public class PlayerStatusManager :MonoBehaviour,IDamageAble
{
    [Header("Player status")]
    [SerializeField] private bool isDead;
    private int hp;
    public bool IsDead => isDead;
	private PlayerData playerData;
	private readonly OnPlayerDamaged onPlayerDamaged = new OnPlayerDamaged();
	private readonly OnPlayerDeath onPlayerDeath = new OnPlayerDeath();

	private void Awake()
	{
		playerData = SaveManager.Instance.GetPlayerData();
		hp = playerData.playerHealth;
	}

	public void TakeDamage(int damage, GameObject d)
    {
		hp -= damage;
		onPlayerDamaged.Damage=damage;
		EventManager.Send(onPlayerDamaged);
		if (!(hp <= 0)) return;
		hp = 0;
		isDead = true;
		EventManager.Send(onPlayerDeath);
    }

}
