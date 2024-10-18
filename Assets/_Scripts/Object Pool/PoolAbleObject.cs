using System.Collections;
using UnityEngine;

namespace Object_Pool
{
	public class PoolAbleObject : MonoBehaviour
	{
		[HideInInspector]
		public ObjectPool Parent;

		protected virtual void Awake()
		{
		}

		protected virtual void OnDisable()
		{
			Parent.ReturnObjectToPool(this);
		}

		protected IEnumerator DisableOnEndCoroutine(float time)
		{
			yield return new WaitForSeconds(time);
			gameObject.SetActive(false);
		}
	}
}
