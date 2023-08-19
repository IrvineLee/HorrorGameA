using UnityEngine;

using Personal.Character.Player;
using Helper;

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

		FPSController fpsController;

		float speedBlend;
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
			if (fpsController.InputDirection == Vector3.zero && speedBlend == 0) return;

			float inputValue = fpsController.InputDirection.z;
			float targetSpeed = fpsController.TargetSpeed;
			float speedChangeRate = fpsController.SpeedChangeRate;

			speedBlend = Mathf.Lerp(speedBlend, inputValue >= 0 ? targetSpeed : -targetSpeed, Time.deltaTime * speedChangeRate);
			if (inputValue >= 0 && speedBlend > -0.01f && speedBlend < 0.01f) speedBlend = 0f;
		}

		void LateUpdate()
		{
			Animator.SetBool(animIDGrounded, fpsController.IsGrounded);
			Animator.SetFloat(animIDSpeed, speedBlend);
			Animator.SetFloat(animIDMotionSpeed, !isSlowdownMotionBlend ? fpsController.InputMagnitude : motionBlend);
		}

		/// <summary>
		/// Used to reset the animation blending values.
		/// </summary>
		public void ResetAnimationBlend(float duration = 0)
		{
			speedAnimationBlendCR?.StopCoroutine();
			speedAnimationBlendCR = CoroutineHelper.LerpWithinSeconds(speedBlend, 0, duration, (result) => speedBlend = result);

			isSlowdownMotionBlend = true;
			slowdownMotionBlendCR?.StopCoroutine();
			slowdownMotionBlendCR = CoroutineHelper.LerpWithinSeconds(fpsController.InputMagnitude, 0, duration, (result) => motionBlend = result, () => isSlowdownMotionBlend = false);
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
			fpsController.OnJumpEvent -= OnJump;
			fpsController.OnFreeFallEvent -= OnFreeFall;
		}
	}
}