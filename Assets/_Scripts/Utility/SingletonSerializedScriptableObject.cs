using Sirenix.OdinInspector;
using UnityEngine;

	public abstract class SingletonSerializedScriptableObject<T> : SerializedScriptableObject where T : SingletonSerializedScriptableObject<T>
	{
		public static T Instance;

		private void Awake()
		{
			Instance = this as T;

			if (this is T instance)
			{
				if (Instance != null && Instance != this)
				{
					Debug.LogWarning($"Instance of type {typeof(T)} already exists");
				}
				else
				{
					Instance = instance;
				}

				return;
			}

			Debug.LogError($"Instance of type {typeof(T)} not found.");
		}

		private void OnValidate()
		{
			if (Instance == null)
			{
				Instance = this as T;
			}
		}
	}
