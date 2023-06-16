using System;
using UnityEngine;

using Personal.Manager;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.FSM.Character;
using Personal.UI.Option;
using Helper;
using static Personal.UI.Option.OptionHandlerUI;

namespace Personal.Character.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class FPSController : GameInitialize
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		[SerializeField] float moveSpeed = 4.0f;

		[Tooltip("Sprint speed of the character in m/s")]
		[SerializeField] float sprintSpeed = 6.0f;

		[Tooltip("Move speed backward of the character in m/s")]
		[SerializeField] float backSpeed = 2.0f;

		[Tooltip("Rotation speed of the character")]
		[SerializeField] float rotationSpeed = 1.0f;

		[Tooltip("Acceleration and deceleration")]
		[SerializeField] float speedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		[SerializeField] float jumpHeight = 1.2f;

		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		[SerializeField] float gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		[SerializeField] float jumpTimeout = 0.1f;

		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		[SerializeField] float fallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		[SerializeField] bool isGrounded = true;

		[Tooltip("Useful for rough ground")]
		[SerializeField] float groundedOffset = -0.14f;

		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		[SerializeField] float groundedRadius = 0.5f;

		[Tooltip("What layers the character uses as ground")]
		[SerializeField] LayerMask groundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		[SerializeField] GameObject cinemachineCameraTarget;

		[Tooltip("How far in degrees can you move the camera up")]
		[SerializeField] float topClamp = 90.0f;

		[Tooltip("How far in degrees can you move the camera down")]
		[SerializeField] float bottomClamp = -90.0f;

		public bool IsGrounded { get => isGrounded; }
		public float SpeedAnimationBlend { get; private set; }
		public float InputMagnitude { get; private set; }
		public CharacterController Controller { get; private set; }

		public event Action<bool> OnJumpEvent;
		public event Action<bool> OnFreeFallEvent;

		// cinemachine
		float _cinemachineTargetPitch;

		// player
		float _speed;
		float _rotationVelocity;
		float _verticalVelocity;
		float _terminalVelocity = 53.0f;

		// timeout deltatime
		float _jumpTimeoutDelta;
		float _fallTimeoutDelta;

		FPSInputController input;
		PlayerStateMachine fsm;
		OptionGameUI optionGameUI;

		bool isInvertedLookHorizontal;
		bool isInvertedLookVertical;

		CoroutineRun speedAnimationBlendCR = new CoroutineRun();
		CoroutineRun inputMagnitudeCR = new CoroutineRun();

		const float _threshold = 0.01f;

		bool IsCurrentDeviceMouse { get => InputManager.Instance.InputDeviceType == InputDeviceType.KeyboardMouse; }

		protected override void Initialize()
		{
			input = InputManager.Instance.FPSInputController;
			fsm = GetComponentInParent<PlayerStateMachine>();
			Controller = GetComponentInChildren<CharacterController>();

			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
			_fallTimeoutDelta = fallTimeout;

			if (!UIManager.Instance.OptionUI.TabDictionary.TryGetValue(MenuTab.Game, out Tab tab)) return;

			optionGameUI = (OptionGameUI)tab.OptionMenuUI;
			optionGameUI.OnCameraSensitivityEvent += SetRotationSpeed;
			optionGameUI.OnInvertLookHorizontalEvent += SetInvertedLookHorizontal;
			optionGameUI.OnInvertLookVerticalEvent += SetInvertedLookVertical;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (!fsm || fsm.IsPlayerThisState(typeof(PlayerIdleState))) return;

			JumpAndGravity();
			GroundedCheck();
			Move();
		}

		void LateUpdate()
		{
			if (!fsm) return;

			CameraRotation();
		}

		void SetRotationSpeed(float value) { rotationSpeed = value; }
		void SetInvertedLookHorizontal(bool isFlag) { isInvertedLookHorizontal = isFlag; }
		void SetInvertedLookVertical(bool isFlag)
		{
			if (isInvertedLookVertical == isFlag) return;

			isInvertedLookVertical = isFlag;
			_cinemachineTargetPitch = -_cinemachineTargetPitch;
		}

		/// <summary>
		/// Used to reset the animation blending values. Typically for dissolve shader.
		/// </summary>
		public void ResetAnimationBlend(float duration = 0)
		{
			speedAnimationBlendCR?.StopCoroutine();
			inputMagnitudeCR?.StopCoroutine();

			speedAnimationBlendCR = CoroutineHelper.LerpWithinSeconds(SpeedAnimationBlend, 0, duration, (result) => SpeedAnimationBlend = result);
			inputMagnitudeCR = CoroutineHelper.LerpWithinSeconds(InputMagnitude, 0, duration, (result) => InputMagnitude = result);
		}

		void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
			isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
		}

		void CameraRotation()
		{
			// if there is an input
			if (input.Look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

				_cinemachineTargetPitch += input.Look.y * rotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = input.Look.x * rotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

				// Update Cinemachine camera target pitch
				cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(!isInvertedLookVertical ? _cinemachineTargetPitch : -_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate((!isInvertedLookHorizontal ? Vector3.up : Vector3.down) * _rotationVelocity);
			}
		}

		void Move()
		{
			// normalise input direction
			Vector3 inputDirection = new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;

			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = input.IsSprint ? sprintSpeed : moveSpeed;

			if (input.Move == Vector2.zero) targetSpeed = 0f;
			else if (inputDirection.z < 0) targetSpeed = backSpeed;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(Controller.velocity.x, 0.0f, Controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			InputMagnitude = 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * InputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			SpeedAnimationBlend = Mathf.Lerp(SpeedAnimationBlend, inputDirection.z >= 0 ? targetSpeed : -targetSpeed, Time.deltaTime * speedChangeRate);
			if (inputDirection.z >= 0 && SpeedAnimationBlend > -0.01f && SpeedAnimationBlend < 0.01f) SpeedAnimationBlend = 0f;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (input.Move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * input.Move.x + transform.forward * input.Move.y;
			}

			// move the player
			Controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		void JumpAndGravity()
		{
			if (isGrounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = fallTimeout;

				OnJumpEvent?.Invoke(false);
				OnFreeFallEvent?.Invoke(false);

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (input.IsJump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
					OnJumpEvent?.Invoke(true);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					OnFreeFallEvent?.Invoke(true);
				}

				// if we are not grounded, do not jump
				//_input.Jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += gravity * Time.deltaTime;
			}
		}

		static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (isGrounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
		}

		void OnApplicationQuit()
		{
			optionGameUI.OnCameraSensitivityEvent -= SetRotationSpeed;
			optionGameUI.OnInvertLookHorizontalEvent -= SetInvertedLookHorizontal;
			optionGameUI.OnInvertLookVerticalEvent -= SetInvertedLookVertical;
		}
	}
}