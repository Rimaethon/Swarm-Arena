using System.Collections.Generic;
using UnityEngine;

namespace Object_Pool
{
	public class ObjectPool : MonoBehaviour
	{
		private Queue<PoolAbleObject> availableObjectsPool;
		private PoolAbleObject prefab;

		public void InitializePool(PoolAbleObject prefab, int size)
		{
			this.prefab = prefab;
			availableObjectsPool = new Queue<PoolAbleObject>();
			CreateObjects(size);
		}

		public PoolAbleObject GetObject(Vector3 position, Quaternion rotation)
		{
			if (availableObjectsPool.Count == 0)
			{
				CreateObjects();
			}

			PoolAbleObject instance = availableObjectsPool.Dequeue();
			instance.transform.position = position;
			instance.transform.rotation = rotation;
			instance.gameObject.SetActive(true);
			return instance;
		}

		public void ReturnObjectToPool(PoolAbleObject pooledObject)
		{
			pooledObject.gameObject.SetActive(false);
			availableObjectsPool.Enqueue(pooledObject);
		}

		private void CreateObjects(int amount = 1)
		{
			for (int i = 0; i < amount; i++)
			{
				PoolAbleObject poolAbleObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
				poolAbleObject.Parent = this;
				poolAbleObject.gameObject.SetActive(false);
			}
		}
	}
}
