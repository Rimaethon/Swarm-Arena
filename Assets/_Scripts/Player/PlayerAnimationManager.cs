using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
	[Header("Animations")]
	[SerializeField] private Rig _rifleRig;
	[SerializeField] private Rig _twoHandedMeleeRig;
	[SerializeField] private AnimationClip _rifleReloadingAnimation;
	[SerializeField] private float _rifleReloadingAnimationOffset = 0.75f;
	private Animator animator;
	private static readonly int rifleWalk = Animator.StringToHash("RifleWalk");
	private static readonly int defaultWalk = Animator.StringToHash("DefaultWalk");
	private static readonly int isAiming = Animator.StringToHash("IsAiming");
	private static readonly int rifleMediumShot = Animator.StringToHash("RifleMediumShot");
	private static readonly int speed = Animator.StringToHash("Speed");
	private static readonly int horizontalSpeed = Animator.StringToHash("HorizontalSpeed");
	private static readonly int verticalSpeed = Animator.StringToHash("VerticalSpeed");
	private PlayerController playerController;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		playerController = GetComponent<PlayerController>();
	}

	private void OnEnable()
	{
		EventManager.RegisterHandler<OnPlayerDeath>(HandleDeathAnimation);
	}

	private void OnDisable()
	{
		EventManager.UnregisterHandler<OnPlayerDeath>(HandleDeathAnimation);
	}

	private void HandleDeathAnimation(OnPlayerDeath obj)
	{
		animator.SetTrigger("Death");
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
		animator.SetTrigger(rifleMediumShot);
	}

	public void PlayReloadAnimation(string animationName)
	{
		StartCoroutine(PlayRifleReloadAnimationCoroutine(animationName));
	}

	private void UpdateAnimation()
	{
		animator.SetFloat(speed, playerController.moveDirection.magnitude, 0.05f, Time.deltaTime);
		animator.SetFloat(horizontalSpeed, playerController.HorizontalSpeed, 0.02f, Time.deltaTime);
		animator.SetFloat(verticalSpeed, playerController.VerticalSpeed, 0.02f, Time.deltaTime);
	}

	private void SetWeaponAnimationPattern()
	{
		SetRifleRig();
		animator.ResetTrigger(defaultWalk);
		animator.SetTrigger(rifleWalk);
		animator.SetBool(isAiming, true);
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
