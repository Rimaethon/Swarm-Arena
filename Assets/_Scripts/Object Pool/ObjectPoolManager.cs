using System.Collections.Generic;
using UnityEngine;

namespace Object_Pool
{
	public class ObjectPoolManager:MonoBehaviour
	{
		private static readonly Dictionary<PoolAbleObject, ObjectPool> objectPools = new Dictionary<PoolAbleObject, ObjectPool>();

		private void OnDestroy()
		{
			objectPools.Clear();
		}

		public static ObjectPool CreateInstance(PoolAbleObject prefab, int size)
		{
			ObjectPool pool;

			if (objectPools.TryGetValue(prefab, out ObjectPool objectPool))
			{
				pool = objectPool;
			}
			else
			{
				GameObject poolObject = new GameObject(prefab + " Pool");
				pool = poolObject.AddComponent<ObjectPool>();
				pool.InitializePool(prefab, size);
				objectPools.Add(prefab, pool);
			}
			return pool;
		}
	}
}
