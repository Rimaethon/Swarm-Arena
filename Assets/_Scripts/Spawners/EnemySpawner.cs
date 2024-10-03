using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	public MeleeAIAgent enemyPrefab;
	public Transform player;
	[SerializeField] float spawnRadius = 20f;
	[SerializeField] float spawnCooldown = 1f;
	private float counter;
	private Camera mainCamera;
	private ObjectPool objectPool;

	private void Start()
	{
		mainCamera = Camera.main;
		counter = spawnCooldown;
		objectPool=ObjectPool.CreateInstance(enemyPrefab, 50);
		SpawnEnemy();
	}

	private void FixedUpdate()
	{
		counter -= Time.fixedDeltaTime;
		if (!(counter <= 0)) return;
		counter = spawnCooldown;
		SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		if (TryGetSpawnPoint(out Vector3 spawnPoint))
		{
			objectPool.GetObject(spawnPoint, Quaternion.identity).GetComponent<BaseAIAgent>().player = player;
		}
	}

	private bool TryGetSpawnPoint(out Vector3 spawnPoint)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomPoint = player.position + Random.insideUnitSphere * spawnRadius;
			randomPoint.y = player.position.y;
			if (!IsValidSpawnPoint(randomPoint)) continue;
			spawnPoint = randomPoint;
			return true;
		}
		spawnPoint = Vector3.zero;
		return false;
	}

	private bool IsValidSpawnPoint(Vector3 point)
	{
		NavMeshHit hit;
		if (!NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
		{
			return false;
		}
		Vector3 viewportPoint = mainCamera.WorldToViewportPoint(point);
		return !(viewportPoint.x >= 0 && viewportPoint is { x: <= 1, y: >= 0 and <= 1, z: > 0 });
	}
}
