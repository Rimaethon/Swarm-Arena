using Event_System;
using Player;
using UI.Buttons;
using UnityEngine;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField]
		private Joystick _joystick;
		private PlayerController playerController;

		private void Awake()
		{
			playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		}

		private void FixedUpdate()
		{
			playerController.moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
		}

		private void OnEnable()
		{
			EventManager.Subscribe<LevelEndEventArgs>(HandlePlayerDeath);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<LevelEndEventArgs>(HandlePlayerDeath);
		}

		private void HandlePlayerDeath(LevelEndEventArgs data)
		{
			_joystick.gameObject.SetActive(false);
		}
	}
}
