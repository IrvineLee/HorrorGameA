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
		CoroutineRun speedAnimationBlendCR = new CoroutineRun();

		float motionBlend;
		bool isSlowdownMotionBlend;
		CoroutineRun slowdownMotionBlendCR = new CoroutineRun();

		protected override void Awake()
		{
			base.Awake();
			AssignAnimationIDs();

			fpsController = GetComponentInParent<FPSController>();

			fpsController.OnJumpEvent += OnJump;
			fpsController.OnFreeFallEvent += OnFreeFall;
		}

		void Update()
		{
			if (fpsController.InputDirection == Vector3.zero && velocity.x == 0 && velocity.z == 0) return;

			Vector3 inputDirection = fpsController.InputDirection;
			float speedChangeRate = fpsController.SpeedChangeRate;

			LerpToVelocity(ref velocity.x, inputDirection.x, fpsController.Velocity.x, speedChangeRate);
			LerpToVelocity(ref velocity.z, inputDirection.z, fpsController.Velocity.z, speedChangeRate);
		}

		void LateUpdate()
		{
			Animator.SetFloat(animIDVelozityX, velocity.x);
			Animator.SetFloat(animIDVelozityZ, velocity.z);
			Animator.SetBool(animIDGrounded, fpsController.IsGrounded);
			Animator.SetFloat(animIDMotionSpeed, !isSlowdownMotionBlend ? fpsController.InputMagnitude : motionBlend);
		}

		/// <summary>
		/// Used to reset the animation blending values.
		/// </summary>
		public void ResetAnimationBlend(float duration = 0)
		{
			speedAnimationBlendCR?.StopCoroutine();
			speedAnimationBlendCR = CoroutineHelper.LerpWithinSeconds(velocity.z, 0, duration, (result) => velocity.z = result);

			isSlowdownMotionBlend = true;
			slowdownMotionBlendCR?.StopCoroutine();
			slowdownMotionBlendCR = CoroutineHelper.LerpWithinSeconds(fpsController.InputMagnitude, 0, duration, (result) => motionBlend = result, () => isSlowdownMotionBlend = false);
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

		void LerpToVelocity(ref float fromVelocity, float inputAxisDirection, float toVelocity, float speedChangeRate)
		{
			fromVelocity = Mathf.Lerp(fromVelocity, inputAxisDirection >= 0 ? toVelocity : -toVelocity, Time.deltaTime * speedChangeRate);
			if (inputAxisDirection >= 0 && fromVelocity > -0.01f && fromVelocity < 0.01f) fromVelocity = 0f;
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