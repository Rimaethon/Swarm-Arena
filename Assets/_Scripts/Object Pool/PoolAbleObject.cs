using System.Collections;
using UnityEngine;

    public class PoolAbleObject : MonoBehaviour
    {
        [HideInInspector] public ObjectPool Parent;

        public virtual void Awake()
        {

        }

        public virtual void OnDisable()
        {
            Parent.ReturnObjectToPool(this);
        }

        protected IEnumerator DisableOnEndCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }
