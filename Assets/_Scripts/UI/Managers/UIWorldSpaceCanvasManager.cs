using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Event_System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Managers
{
	public class UIWorldSpaceCanvasManager : MonoBehaviour
	{
		[SerializeField]
		private RectTransform bulletCountView;
		[SerializeField]
		private RectTransform damageTextView;
		[SerializeField]
		private GameObject damageTextPrefab;
		[SerializeField]
		private int damageTextPoolSize = 10;
		[SerializeField]
		private Vector3 bulletCountViewOffset = new Vector3(0, 0, 0);
		private readonly Queue<GameObject> damageTexts = new Queue<GameObject>();
		private TextMeshProUGUI bulletCountText;
		private Transform player;

		private void Awake()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
			bulletCountText = bulletCountView.GetComponentInChildren<TextMeshProUGUI>();

			for (int i = 0; i < damageTextPoolSize; i++)
			{
				damageTexts.Enqueue(Instantiate(damageTextPrefab, transform));
			}
		}

		private void LateUpdate()
		{
			bulletCountView.position = player.position + bulletCountViewOffset;
		}

		private void OnEnable()
		{
			EventManager.Subscribe<EnemyDamagedEventArgs>(ShowDamage);
			EventManager.Subscribe<BulletCountChangedEventArgs>(ChangeBulletCount);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<EnemyDamagedEventArgs>(ShowDamage);
			EventManager.UnSubscribe<BulletCountChangedEventArgs>(ChangeBulletCount);
		}

		private void ShowDamage(EnemyDamagedEventArgs data)
		{
			StartCoroutine(DamageTextCoroutine(data.Position, data.Damage));
		}

		private IEnumerator DamageTextCoroutine(Vector3 position, float damage)
		{
			if (damageTexts.Count == 0)
			{
				damageTexts.Enqueue(Instantiate(damageTextPrefab, transform));
			}

			RectTransform textRectTransform = damageTexts.Dequeue().GetComponent<RectTransform>();
			textRectTransform.GetComponent<TextMeshProUGUI>().text = damage.ToString();
			textRectTransform.position = position;
			textRectTransform.gameObject.SetActive(true);
			float x = Random.Range(0f, 2f);
			float y = Random.Range(2, 4.5f);
			textRectTransform.DOMoveY(position.y + y, 1);
			textRectTransform.DOMoveX(position.x + x, 1);
			yield return new WaitForSeconds(1);
			textRectTransform.gameObject.SetActive(false);
			damageTexts.Enqueue(textRectTransform.gameObject);
		}

		private void ChangeBulletCount(BulletCountChangedEventArgs bulletCountChangedEventArgs)
		{
			bulletCountText.text = bulletCountChangedEventArgs.bulletCount.ToString();
		}
	}
}
