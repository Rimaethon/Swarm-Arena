using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Managers
{
	public class CoinSpawner:MonoBehaviour
	{
		[SerializeField] private GoldCoin coinPrefab;
		private ObjectPool coinPool;
		private Transform playerTransform;
		private const float min_speed = 25;
		private const float max_speed = 80;
		private readonly WaitForSeconds animationWaitForSeconds= new WaitForSeconds(3.5f);

		private void Awake()
		{
			coinPool = ObjectPool.CreateInstance(coinPrefab, 50);
			playerTransform = FindObjectOfType<PlayerController>().transform;
		}

		private void OnEnable()
		{
			EventManager.RegisterHandler<OnEnemyKilled>(SpawnCoin);
		}

		private void OnDisable()
		{
			EventManager.UnregisterHandler<OnEnemyKilled>(SpawnCoin);
		}

		private void SpawnCoin(OnEnemyKilled data)
		{
			PoolAbleObject coin = coinPool.GetObject(data.position , Quaternion.identity);
			coin.transform.rotation = Quaternion.Euler(90, 0, 0);
			StartCoroutine(CoinAnimation(playerTransform, coin));
		}

		private IEnumerator CoinAnimation(Transform playerTransform,PoolAbleObject coin)
		{
			coin.transform.DOMoveY(coin.transform.position.y +0.5f, 3.5f).SetLoops(1, LoopType.Yoyo);
			coin.transform.DORotate(new Vector3(90, 720, 0), 3.5f, RotateMode.FastBeyond360).SetLoops(1, LoopType.Restart);
			yield return animationWaitForSeconds;

			float distance = Vector3.Distance(coin.transform.position, playerTransform.position);
			while ( distance > 0.2f)
			{
				float speed = Mathf.Clamp(distance, min_speed, max_speed)*Time.deltaTime;
				coin.transform.position = Vector3.MoveTowards(coin.transform.position, playerTransform.position, speed);
				distance = Vector3.Distance(coin.transform.position, playerTransform.position);
				yield return null;
			}
			coin.gameObject.SetActive(false);
		}
	}
}
