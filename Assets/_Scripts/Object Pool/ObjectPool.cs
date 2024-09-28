using System.Collections.Generic;
using UnityEngine;


	public class ObjectPool : MonoBehaviour
	{
		[HideInInspector] public PoolAbleObject Prefab;
		[HideInInspector] public int Size;
		[HideInInspector] public List<PoolAbleObject> AvailableObjectsPool;
		private GameObject parent;
		private static readonly Dictionary<PoolAbleObject, ObjectPool> objectPools = new Dictionary<PoolAbleObject, ObjectPool>();

		~ObjectPool()
		{
			objectPools.Clear();
		}

		public static ObjectPool CreateInstance(PoolAbleObject Prefab, int Size)
		{
			ObjectPool pool;

			if (objectPools.TryGetValue(Prefab, out ObjectPool objectPool))
			{
				pool = objectPool;
			}
			else
			{
				GameObject poolObject = new GameObject(Prefab + " Pool");
				pool= poolObject.AddComponent<ObjectPool>();
				pool.parent = poolObject;
				pool.Prefab = Prefab;
				pool.Size = Size;
				pool.AvailableObjectsPool = new List<PoolAbleObject>();
				pool.CreateObjects();
				objectPools.Add(Prefab, pool);
			}
			return pool;
		}

		private void CreateObjects()
		{
			for (int i = 0; i < Size; i++)
			{
				CreateObject();
			}
		}

		private void CreateObject()
		{
			PoolAbleObject poolAbleObject = Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
			poolAbleObject.Parent = this;
			poolAbleObject.gameObject.SetActive(false);
		}

		public PoolAbleObject GetObject(Vector3 position, Quaternion rotation)
		{
			if (AvailableObjectsPool.Count == 0)
			{
				CreateObject();
			}

			PoolAbleObject instance = AvailableObjectsPool[0];
			AvailableObjectsPool.RemoveAt(0);
			instance.transform.position = position;
			instance.transform.rotation = rotation;
			instance.gameObject.SetActive(true);
			return instance;
		}

		public void ReturnObjectToPool(PoolAbleObject pooledObject)
		{
			pooledObject.gameObject.SetActive(false);
			AvailableObjectsPool.Add(pooledObject);
		}
	}
