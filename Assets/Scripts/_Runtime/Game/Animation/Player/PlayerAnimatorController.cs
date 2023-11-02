using UnityEngine;

using Personal.Character.Player;
using Helper;

namespace Personal.Character.Animation
{
	public class PlayerAnimatorController : AnimatorController
	{
		// Animation IDs
		int animIDVelozityX;
		int animIDVelozityZ;
		int animIDGrounded;
		int animIDJump;
		int animIDFreeFall;
		int animIDMotionSpeed;

		FPSController fpsController;

		Vector3 velocity;
		bool isReset;

		CoroutineRun speedAnimationBlendCR = new CoroutineRun();

		protected override void EarlyInitialize()
		{
			base.EarlyInitialize();
			AssignAnimationIDs();

			fpsController = GetComponentInParent<FPSController>();

			fpsController.OnJumpEvent += OnJump;
			fpsController.OnFreeFallEvent += OnFreeFall;
		}

		void Update()
		{
			if (isReset) return;

			Vector2 inputDirection = fpsController.InputDirection;
			if (inputDirection == Vector2.zero && velocity == Vector3.zero) return;

			LerpToVelocity(ref velocity.x, inputDirection.x, fpsController.Velocity.x);
			LerpToVelocity(ref velocity.z, inputDirection.y, fpsController.Velocity.z);
		}

		void LateUpdate()
		{
			Animator.SetFloat(animIDVelozityX, velocity.x);
			Animator.SetFloat(animIDVelozityZ, velocity.z);
			Animator.SetBool(animIDGrounded, fpsController.IsGrounded);
			Animator.SetFloat(animIDMotionSpeed, fpsController.InputMagnitude);
		}

		/// <summary>
		/// Used to reset the animation blending values.
		/// </summary>
		public void ResetAnimationBlend(float duration = 0)
		{
			isReset = true;
			speedAnimationBlendCR?.StopCoroutine();
			speedAnimationBlendCR = CoroutineHelper.LerpWithinSeconds(velocity, Vector3.zero, duration, (result) => velocity = result, () => isReset = false);
		}

		void AssignAnimationIDs()
		{
			animIDVelozityX = Animator.StringToHash("VelocityX");
			animIDVelozityZ = Animator.StringToHash("VelocityZ");
			animIDGrounded = Animator.StringToHash("Grounded");
			animIDJump = Animator.StringToHash("Jump");
			animIDFreeFall = Animator.StringToHash("FreeFall");
			animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		void LerpToVelocity(ref float fromVelocity, float inputAxisDirection, float toVelocity)
		{
			float toValue = 0;
			if (inputAxisDirection != 0)
			{
				toValue = inputAxisDirection >= 0 ? toVelocity : -toVelocity;
			}

			fromVelocity = Mathf.Lerp(fromVelocity, toValue, Time.deltaTime * fpsController.SpeedChangeRate);
			if (inputAxisDirection == 0 && fromVelocity.IsWithin(-0.01f, 0.01f)) fromVelocity = 0;
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
			fpsController.OnJumpEvent -= OnJump;
			fpsController.OnFreeFallEvent -= OnFreeFall;
		}
	}
}