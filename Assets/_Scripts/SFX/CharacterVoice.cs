using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterVoice : MonoBehaviour
{
	[SerializeField] private List<AudioClip> _idleVoice;
	[SerializeField] private List<AudioClip> _agroVoice;
	[SerializeField] private List<AudioClip> _deadVoice;
	[SerializeField] private List<AudioClip> _painEffectVoice;
	[SerializeField] private float _idleVoiceTime;

	private IDamageAble damageAble;
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		damageAble = GetComponent<IDamageAble>();
		StartCoroutine(WaitCoroutine(_idleVoiceTime));
	}

	private void OnEnable()
	{
		//damageAble.OnDeath += PlayDeadVoice;
	}

	private void OnDisable()
	{
		//damageAble.OnDeath -= PlayDeadVoice;
		StopAllCoroutines();
	}

	public void PlayIdleVoice()
	{
		audioSource.pitch = Random.Range(0.8f, 1.1f);
		audioSource.PlayOneShot(_idleVoice[Random.Range(0, _idleVoice.Count)]);
	}

	public void PlayDeadVoice()
	{
		audioSource.pitch = Random.Range(0.8f, 1.1f);
		audioSource.PlayOneShot(_deadVoice[Random.Range(0, _deadVoice.Count)]);
	}

	private IEnumerator WaitCoroutine(float delay)
	{
		while (!damageAble.IsDead)
		{
			yield return new WaitForSeconds(delay + delay * Random.Range(0, 4));

			if(!damageAble.IsDead)
				PlayIdleVoice();
		}

		yield break;
	}
}
