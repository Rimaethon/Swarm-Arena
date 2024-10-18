using UnityEngine;

namespace Player
{
	public static class AnimationHashData
	{
		public static readonly int RifleWalk = Animator.StringToHash("RifleWalk");
		public static readonly int DefaultWalk = Animator.StringToHash("DefaultWalk");
		public static readonly int IsAiming = Animator.StringToHash("IsAiming");
		public static readonly int RifleMediumShot = Animator.StringToHash("RifleMediumShot");
		public static readonly int Speed = Animator.StringToHash("Speed");
		public static readonly int HorizontalSpeed = Animator.StringToHash("HorizontalSpeed");
		public static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
		public static readonly int Death = Animator.StringToHash("Death");
		public static readonly int LightAttack = Animator.StringToHash("LightAttack");
		public static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");
		public static readonly int RifleReload = Animator.StringToHash("RifleReload");
	}
}
