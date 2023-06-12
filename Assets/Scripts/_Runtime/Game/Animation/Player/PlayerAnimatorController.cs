using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character.Player;
using Personal.Manager;

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

		protected override async UniTask Awake()
		{
			await base.Awake();

			AssignAnimationIDs();

			fPSController = StageManager.Instance.PlayerController.FSM.FPSController;
			fPSController.OnJumpEvent += OnJump;
			fPSController.OnFreeFallEvent += OnFreeFall;
		}

		void LateUpdate()
		{
			animator.SetBool(animIDGrounded, fPSController.IsGrounded);
			animator.SetFloat(animIDSpeed, fPSController.SpeedAnimationBlend);
			animator.SetFloat(animIDMotionSpeed, fPSController.InputMagnitude);
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
			animator.SetBool(animIDJump, isFlag);
		}

		void OnFreeFall(bool isFlag)
		{
			animator.SetBool(animIDFreeFall, isFlag);
		}

		void OnApplicationQuit()
		{
			fPSController.OnJumpEvent -= OnJump;
			fPSController.OnFreeFallEvent -= OnFreeFall;
		}
	}
}