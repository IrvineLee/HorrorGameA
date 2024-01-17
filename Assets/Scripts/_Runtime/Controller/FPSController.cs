using System;
using UnityEngine;

using Sirenix.OdinInspector;
using Personal.Manager;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.FSM.Character;
using Personal.Setting.Game;

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

		[Tooltip("Move speed horizontally of the character in m/s")]
		[SerializeField] float horizontalSpeed = 2.0f;

		[Tooltip("Rotation speed of the character")]
		[SerializeField] [ReadOnly] float rotationSpeed = 1.0f;

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
		public float InputMagnitude { get; private set; }
		public Vector2 InputDirection { get => input.MoveNormalized; }
		public Vector3 Velocity { get => velocity; }
		public float SpeedChangeRate { get => speedChangeRate; }
		public CharacterController Controller { get; private set; }
		public GameObject CinemachineCameraGO { get => cinemachineCameraTarget; }
		public float CinemachineTargetPitch { get => cinemachineTargetPitch; }

		public event Action<bool> OnJumpEvent;
		public event Action<bool> OnFreeFallEvent;

		// cinemachine
		float cinemachineTargetPitch;

		// player
		float speed;
		float rotationVelocity;
		float verticalVelocity;
		float terminalVelocity = 53.0f;

		// timeout deltatime
		float jumpTimeoutDelta;
		float fallTimeoutDelta;

		Vector3 velocity;

		FPSInputController input;
		PlayerStateMachine fsm;

		GameData gameData;
		bool isCurrentInvertedLookVertical;

		bool isSprint;
		bool isJump;

		protected override void Initialize()
		{
			input = InputManager.Instance.FPSInputController;

			Controller = GetComponentInChildren<CharacterController>();
			fsm = GetComponentInParent<PlayerStateMachine>();

			// reset our timeouts on start
			jumpTimeoutDelta = jumpTimeout;
			fallTimeoutDelta = fallTimeout;

			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;

			rotationSpeed = gameData.CameraSensitivity;
			isCurrentInvertedLookVertical = gameData.IsInvertLookVertical;
		}

		void Update()
		{
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

		public void Sprint(bool isFlag) { isSprint = isFlag; }
		public void Jump(bool isFlag) { isJump = isFlag; }

		/// <summary>
		/// Call this when you need to update the camera y value outside of normal mouse movement. Ex: During an lookat event.
		/// </summary>
		/// <param name="toValue"></param>
		/// <param name="targetY"></param>
		public void UpdateTargetPitch(float toValue)
		{
			if (toValue > 180) toValue -= 360;

			cinemachineTargetPitch = toValue;
			cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(!gameData.IsInvertLookVertical ? cinemachineTargetPitch : -cinemachineTargetPitch, 0.0f, 0.0f);
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
			if (input.Look == Vector2.zero) return;

			//Don't multiply mouse input by Time.deltaTime
			float deltaTimeMultiplier = InputManager.Instance.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

			cinemachineTargetPitch += input.Look.y * gameData.CameraSensitivity * deltaTimeMultiplier;
			rotationVelocity = input.Look.x * gameData.CameraSensitivity * deltaTimeMultiplier;

			// clamp our pitch rotation
			cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

			if (isCurrentInvertedLookVertical != gameData.IsInvertLookVertical)
			{
				cinemachineTargetPitch = -cinemachineTargetPitch;
				isCurrentInvertedLookVertical = gameData.IsInvertLookVertical;
			}

			// Update Cinemachine camera target pitch
			cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(!gameData.IsInvertLookVertical ? cinemachineTargetPitch : -cinemachineTargetPitch, 0.0f, 0.0f);

			// rotate the player left and right
			transform.Rotate((!gameData.IsInvertLookHorizontal ? Vector3.up : Vector3.down) * rotationVelocity);
		}

		void Move()
		{
			// normalise input direction
			Vector3 inputDirection = new Vector3(input.MoveNormalized.x, 0, input.MoveNormalized.y);

			// handle horizontal move speed
			if (inputDirection.x != 0) velocity.x = horizontalSpeed;

			if (inputDirection.z != 0)
			{
				// set target speed based on move speed, sprint speed and if sprint is pressed
				velocity.z = isSprint ? sprintSpeed : moveSpeed;
				if (inputDirection.z < 0) velocity.z = backSpeed;
			}
			else
			{
				velocity.z = 0;
			}

			if (inputDirection == Vector3.zero) velocity = Vector3.zero;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(Controller.velocity.x, 0.0f, Controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			InputMagnitude = 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < velocity.z - speedOffset || currentHorizontalSpeed > velocity.z + speedOffset ||
				currentHorizontalSpeed < velocity.x - speedOffset || currentHorizontalSpeed > velocity.x + speedOffset)
			{
				float value = velocity.z == 0 ? velocity.x : velocity.z;

				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				speed = Mathf.Lerp(currentHorizontalSpeed, value * InputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				speed = Mathf.Round(speed * 1000f) / 1000f;
			}
			else
			{
				speed = velocity.z;
			}

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (input.Move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * input.Move.x + transform.forward * input.Move.y;
			}

			// move the player
			if (!Controller.enabled) return;
			Controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
		}

		void JumpAndGravity()
		{
			if (isGrounded)
			{
				// reset the fall timeout timer
				fallTimeoutDelta = fallTimeout;

				OnJumpEvent?.Invoke(false);
				OnFreeFallEvent?.Invoke(false);

				// stop our velocity dropping infinitely when grounded
				if (verticalVelocity < 0.0f)
				{
					verticalVelocity = -2f;
				}

				// Jump
				if (isJump && jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
					OnJumpEvent?.Invoke(true);
				}

				// jump timeout
				if (jumpTimeoutDelta >= 0.0f)
				{
					jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (fallTimeoutDelta >= 0.0f)
				{
					fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					OnFreeFallEvent?.Invoke(true);
				}

				// if we are not grounded, do not jump
				//_input.Jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (verticalVelocity < terminalVelocity)
			{
				verticalVelocity += gravity * Time.deltaTime;
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
	}
}