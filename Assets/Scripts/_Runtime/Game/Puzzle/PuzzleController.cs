using Personal.Manager;
using UnityEngine;

namespace Personal.Puzzle
{
	public class PuzzleController : MonoBehaviour
	{
		public enum PuzzleState
		{
			None = 0,
			Completed,
			Failed,
		}

		protected PuzzleState puzzleState = PuzzleState.None;
		protected PuzzleGamepadMovement puzzleGamepadMovement;

		protected virtual void Awake()
		{
			puzzleGamepadMovement = GetComponentInChildren<PuzzleGamepadMovement>();
		}

		void OnEnable()
		{
			InputManager.OnDeviceIconChanged += HandlePhysicsRaycaster;
		}

		/// <summary>
		/// Handles whether to display gamepad movement or mouse movement.
		/// </summary>
		/// <param name="isFlag"></param>
		protected void HandleMouseOrGamepadDisplay(bool isFlag)
		{
			HandlePhysicsRaycaster();
			EnableGamepadMovement(isFlag);
		}

		void HandlePhysicsRaycaster()
		{
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				StageManager.Instance.CameraHandler.PhysicsRaycaster.enabled = true;
				return;
			}
			StageManager.Instance.CameraHandler.PhysicsRaycaster.enabled = false;
		}

		void EnableGamepadMovement(bool isFlag) { puzzleGamepadMovement.enabled = isFlag; }

		void OnDisable()
		{
			InputManager.OnDeviceIconChanged -= HandlePhysicsRaycaster;
			StageManager.Instance.CameraHandler.PhysicsRaycaster.enabled = false;
		}
	}
}
