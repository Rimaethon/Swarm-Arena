using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIWorldSpaceCanvasManager : MonoBehaviour
{
	[SerializeField] private RectTransform bulletCountView;
	[SerializeField] private RectTransform damageTextView;
	[SerializeField] private GameObject damageTextPrefab;
	[SerializeField] int damageTextPoolSize = 10;
	[SerializeField] Vector3 bulletCountViewOffset = new Vector3(0, 0, 0);
	private TextMeshProUGUI bulletCountText;
	private readonly Queue<GameObject> damageTexts = new Queue<GameObject>();
	private Transform player;

	private void OnEnable()
	{
		EventManager.RegisterHandler<OnDamage>(ShowDamage);
		EventManager.RegisterHandler<OnBulletCountChanged>(ChangeBulletCount);
	}

	private void OnDisable()
	{
		EventManager.UnregisterHandler<OnDamage>(ShowDamage);
		EventManager.UnregisterHandler<OnBulletCountChanged>(ChangeBulletCount);
	}

	private void Awake()
	{
		player= GameObject.FindGameObjectWithTag("Player").transform;
		bulletCountText=bulletCountView.GetComponentInChildren<TextMeshProUGUI>();
		for (int i = 0; i < damageTextPoolSize; i++)
		{
			damageTexts.Enqueue(Instantiate(damageTextPrefab, transform));
		}
	}

	private void LateUpdate()
	{
		bulletCountView.position = player.position + bulletCountViewOffset;
	}

	private void ShowDamage(OnDamage data)
	{
		StartCoroutine(DamageTextCoroutine(data.Position, data.Damage));
	}

	private IEnumerator DamageTextCoroutine(Vector3 position, float damage)
	{
		RectTransform textRectTransform = damageTexts.Dequeue().GetComponent<RectTransform>();
		textRectTransform.GetComponent<TextMeshProUGUI>().text = damage.ToString();
		textRectTransform.position = position;
		textRectTransform.gameObject.SetActive(true);
		float x = Random.Range(0f, 2f);
		float y = Random.Range(2, 4.5f);
		textRectTransform.DOMoveY(position.y +y, 1);
		textRectTransform.DOMoveX(position.x + x, 1);
		yield return new WaitForSeconds(1);
		textRectTransform.gameObject.SetActive(false);
		damageTexts.Enqueue(textRectTransform.gameObject);
	}

	private void ChangeBulletCount(OnBulletCountChanged onBulletCountChanged)
	{
		bulletCountText.text = onBulletCountChanged.bulletCount.ToString();
	}
}
