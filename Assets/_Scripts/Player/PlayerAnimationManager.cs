using System.Collections;
using Event_System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimationManager : MonoBehaviour
	{
		[SerializeField]
		private Rig _rifleRig;
		[SerializeField]
		private float rifleReloadAnimationDuration = 2.5f;
		private Animator animator;
		private PlayerController playerController;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			playerController = GetComponent<PlayerController>();
		}

		private void Start()
		{
			SetWeaponAnimationPattern();
		}

		private void Update()
		{
			UpdateAnimation();
		}

		private void OnEnable()
		{
			EventManager.Subscribe<LevelEndEventArgs>(HandleDeathAnimation);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<LevelEndEventArgs>(HandleDeathAnimation);
		}

		private void HandleDeathAnimation(LevelEndEventArgs obj)
		{
			if (obj.isLevelCompleted)
			{
				return;
			}

			animator.SetTrigger(AnimationHashData.Death);
		}

		public void PlayRifleMediumShot()
		{
			animator.SetTrigger(AnimationHashData.RifleMediumShot);
		}

		public void PlayReloadAnimation()
		{
			StartCoroutine(PlayRifleReloadAnimationCoroutine());
		}

		private void UpdateAnimation()
		{
			animator.SetFloat(AnimationHashData.Speed, playerController.moveDirection.magnitude, 0.05f, Time.deltaTime);
			animator.SetFloat(AnimationHashData.HorizontalSpeed, playerController.HorizontalSpeed, 0.02f, Time.deltaTime);
			animator.SetFloat(AnimationHashData.VerticalSpeed, playerController.VerticalSpeed, 0.02f, Time.deltaTime);
		}

		private void SetWeaponAnimationPattern()
		{
			_rifleRig.weight = 1f;
			animator.ResetTrigger(AnimationHashData.DefaultWalk);
			animator.SetTrigger(AnimationHashData.RifleWalk);
			animator.SetBool(AnimationHashData.IsAiming, true);
		}

		private IEnumerator PlayRifleReloadAnimationCoroutine()
		{
			_rifleRig.weight = 0f;
			animator.CrossFade(AnimationHashData.RifleReload, 0.1f);
			yield return new WaitForSeconds(rifleReloadAnimationDuration);
			_rifleRig.weight = 1f;
		}
	}
}
