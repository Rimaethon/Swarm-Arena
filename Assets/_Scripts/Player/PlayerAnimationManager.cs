using System.Collections;
using Event_System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimationManager : MonoBehaviour
	{
		[SerializeField] private Rig _rifleRig;
		[SerializeField] private Rig _twoHandedMeleeRig;
		[SerializeField] private AnimationClip _rifleReloadingAnimation;
		[SerializeField] private float _rifleReloadingAnimationOffset = 0.75f;
		private Animator animator;
		private PlayerController playerController;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			playerController = GetComponent<PlayerController>();
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
			if(obj.isLevelCompleted)
				return;
			animator.SetTrigger(AnimationHashData.Death);
		}

		private void Start()
		{
			SetWeaponAnimationPattern();
		}

		private void Update()
		{
			UpdateAnimation();
		}

		public void PlayRifleMediumShot()
		{
			animator.SetTrigger(AnimationHashData.RifleMediumShot);
		}

		public void PlayReloadAnimation(string animationName)
		{
			StartCoroutine(PlayRifleReloadAnimationCoroutine(animationName));
		}

		private void UpdateAnimation()
		{
			animator.SetFloat(AnimationHashData.Speed, playerController.moveDirection.magnitude, 0.05f, Time.deltaTime);
			animator.SetFloat(AnimationHashData.HorizontalSpeed, playerController.HorizontalSpeed, 0.02f, Time.deltaTime);
			animator.SetFloat(AnimationHashData.VerticalSpeed, playerController.VerticalSpeed, 0.02f, Time.deltaTime);
		}

		private void SetWeaponAnimationPattern()
		{
			SetRifleRig();
			animator.ResetTrigger(AnimationHashData.DefaultWalk);
			animator.SetTrigger(AnimationHashData.RifleWalk);
			animator.SetBool(AnimationHashData.IsAiming, true);
		}

		private IEnumerator PlayRifleReloadAnimationCoroutine(string animationName)
		{
			SetDefaultRig();
			animator.CrossFade(animationName, 0.1f);
			yield return new WaitForSeconds(_rifleReloadingAnimation.length - _rifleReloadingAnimationOffset);
			SetRifleRig();
		}

		private void SetRifleRig()
		{
			_twoHandedMeleeRig.weight = 0f;
			_rifleRig.weight = 1f;
		}

		private void SetDefaultRig()
		{
			_twoHandedMeleeRig.weight = 0f;
			_rifleRig.weight = 0f;
		}
	}
}
