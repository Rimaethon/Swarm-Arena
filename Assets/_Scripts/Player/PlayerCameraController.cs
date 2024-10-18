using DG.Tweening;
using Event_System;
using UnityEngine;

namespace Camera
{
	[ExecuteAlways]
	public class PlayerCameraController : MonoBehaviour
	{
		private const float start_rotation = 40;
		[SerializeField]
		private Vector3 offset = new Vector3(0, 16, -16);
		[SerializeField]
		private float cameraShakeDuration = 0.5f;
		private Transform player;

		private void Awake()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
			transform.rotation = Quaternion.Euler(start_rotation, 0, 0);
		}

		private void LateUpdate()
		{
			Vector3 desiredPosition = player.position + offset;
			transform.position = desiredPosition;
		}

		private void OnEnable()
		{
			EventManager.Subscribe<PlayerDamagedEventArgs>(ImpactShake);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<PlayerDamagedEventArgs>(ImpactShake);
		}

		private void ImpactShake(PlayerDamagedEventArgs args)
		{
			transform.DOComplete();
			transform.DOShakePosition(cameraShakeDuration);
			transform.DOShakeRotation(cameraShakeDuration, 1);
		}
	}
}
