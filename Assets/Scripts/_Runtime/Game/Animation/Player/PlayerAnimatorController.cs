using UnityEngine;

using Personal.Character.Player;

namespace Personal.Character.Animation
{
	public class PlayerAnimatorController : AnimatorController
	{
		// Animation IDs
		int animIDSpeed;
		int animIDGrounded;
		int animIDJump;
		int animIDFreeFall;
		int animIDMotionSpeed;

		FPSController fPSController;

		protected override void Awake()
		{
			base.Awake();
			AssignAnimationIDs();

			fPSController = GetComponentInParent<FPSController>();
			fPSController.OnJumpEvent += OnJump;
			fPSController.OnFreeFallEvent += OnFreeFall;
		}

		void LateUpdate()
		{
			Animator.SetBool(animIDGrounded, fPSController.IsGrounded);
			Animator.SetFloat(animIDSpeed, fPSController.SpeedAnimationBlend);
			Animator.SetFloat(animIDMotionSpeed, fPSController.InputMagnitude);
		}

		void AssignAnimationIDs()
		{
			animIDSpeed = Animator.StringToHash("Speed");
			animIDGrounded = Animator.StringToHash("Grounded");
			animIDJump = Animator.StringToHash("Jump");
			animIDFreeFall = Animator.StringToHash("FreeFall");
			animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		void OnJump(bool isFlag)
		{
			Animator.SetBool(animIDJump, isFlag);
		}

		void OnFreeFall(bool isFlag)
		{
			Animator.SetBool(animIDFreeFall, isFlag);
		}

		void OnApplicationQuit()
		{
			fPSController.OnJumpEvent -= OnJump;
			fPSController.OnFreeFallEvent -= OnFreeFall;
		}
	}
}