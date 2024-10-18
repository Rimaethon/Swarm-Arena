using AI;
using Object_Pool;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Spawners
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField]
		private MeleeAIAgent enemyPrefab;
		[SerializeField]
		private float spawnRadius = 20f;
		[SerializeField]
		private float spawnCooldown = 1f;
		private float counter;
		private UnityEngine.Camera mainCamera;
		private ObjectPool objectPool;
		private Transform playerTransform;

		private void Start()
		{
			playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
			mainCamera = UnityEngine.Camera.main;
			counter = spawnCooldown;
			objectPool = ObjectPoolManager.CreateInstance(enemyPrefab, 50);
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
				objectPool.GetObject(spawnPoint, Quaternion.identity).GetComponent<BaseAIAgent>().playerTransform = playerTransform;
			}
		}

		private bool TryGetSpawnPoint(out Vector3 spawnPoint)
		{
			Vector3 playerPosition = playerTransform.position;

			for (int i = 0; i < 30; i++)
			{
				Vector3 randomPoint = playerPosition + Random.insideUnitSphere * spawnRadius;
				randomPoint.y = playerPosition.y;
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
}
