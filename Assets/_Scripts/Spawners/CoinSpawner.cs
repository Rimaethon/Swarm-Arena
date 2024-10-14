using System.Collections;
using DG.Tweening;
using Event_System;
using Managers;
using Object_Pool;
using Player;
using UnityEngine;

namespace Spawners
{
	//Also it can give different things based on the enemy type in the future like health, ammo etc.That would make it more like a DropSpawner class
	public class CoinSpawner:MonoBehaviour
	{
		[SerializeField] private GoldCoin coinPrefab;
		[SerializeField] private AnimationCurve coinSpeedCurve;
		private ObjectPool coinPool;
		private Transform playerTransform;
		private const float max_speed = 10;
		private const float coin_start_y = 0.75f;
		private readonly WaitForSeconds animationWaitForSeconds= new WaitForSeconds(3.5f);

		private void Awake()
		{
			coinPool = ObjectPool.CreateInstance(coinPrefab, 50);
			playerTransform = FindObjectOfType<PlayerController>().transform;
		}

		private void OnEnable()
		{
			EventManager.Subscribe<EnemyDamagedEventArgs>(SpawnCoin);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<EnemyDamagedEventArgs>(SpawnCoin);
		}

		private void SpawnCoin(EnemyDamagedEventArgs data)
		{
			if(!data.isDead)
				return;
			PoolAbleObject coin = coinPool.GetObject(new Vector3(data.Position.x,coin_start_y,data.Position.z) , Quaternion.identity);
			coin.transform.rotation = Quaternion.Euler(90, 0, 0);
			StartCoroutine(CoinAnimation(playerTransform, coin));
		}

		private IEnumerator CoinAnimation(Transform playerTransform,PoolAbleObject coin)
		{
			Vector3 coinPosition = coin.transform.position;
			coin.transform.DOMoveY(coinPosition.y +0.5f, 3.5f).SetLoops(1, LoopType.Yoyo);
			coin.transform.DORotate(new Vector3(90, 720, 0), 3.5f, RotateMode.FastBeyond360).SetLoops(1, LoopType.Restart);
			yield return animationWaitForSeconds;

			float time = Time.deltaTime;
			while (Vector3.Distance(coinPosition, playerTransform.position) > 0.2f)
			{
				float speed = coinSpeedCurve.Evaluate(time)*max_speed;
				time += Time.deltaTime;
				coinPosition= Vector3.MoveTowards(coin.transform.position, playerTransform.position, speed);
				coin.transform.position = coinPosition;
				yield return null;
			}
			coin.gameObject.SetActive(false);
		}
	}
}
