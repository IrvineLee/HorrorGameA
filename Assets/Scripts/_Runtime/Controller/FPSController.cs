using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using Personal.Manager;
using Personal.GameState;
using Personal.InputProcessing;
using Cysharp.Threading.Tasks;
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
		[SerializeField] bool grounded = true;

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

		CharacterController _controller;
		FPSInputController _input;
		GameData gameData;

		const float _threshold = 0.01f;

		bool IsCurrentDeviceMouse { get => InputManager.Instance.InputDeviceType == InputDeviceType.KeyboardMouse; }

		protected override async UniTask Awake()
		{
			await base.Awake();

			_input = InputManager.Instance.FPSInputController;
			_controller = GetComponent<CharacterController>();

			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
			_fallTimeoutDelta = fallTimeout;

			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;
			rotationSpeed = gameData.CameraSensitivity;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			JumpAndGravity();
			GroundedCheck();
			Move();
		}

		void LateUpdate()
		{
			CameraRotation();
		}

		public void UpdateRotationSpeed(float value)
		{
			rotationSpeed = value;
		}

		void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
			grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
		}

		void CameraRotation()
		{
			// if there is an input
			if (_input.Look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

				_cinemachineTargetPitch += _input.Look.y * rotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.Look.x * rotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

				// Update Cinemachine camera target pitch
				cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.IsSprint ? sprintSpeed : moveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.Move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.Move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.Move.x + transform.forward * _input.Move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		void JumpAndGravity()
		{
			if (grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = fallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.IsJump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

			if (grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
		}
	}
}