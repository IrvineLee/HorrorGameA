using UnityEngine;
using UnityEngine.InputSystem;

using Personal.Manager;
using Helper;

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

		protected CoroutineRun slideCR = new CoroutineRun();

		protected virtual void Awake()
		{
			puzzleGamepadMovement = GetComponentInChildren<PuzzleGamepadMovement>();
		}

		void OnEnable()
		{
			InputManager.OnDeviceIconChanged += HandlePhysicsRaycaster;
		}

		void Update()
		{
			if (!InputManager.Instance.IsInteract) return;
			if (!slideCR.IsDone) return;

			// Check puzzle click.
			Transform target = puzzleGamepadMovement ? GetActiveSelectionForGamepad() : null;
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				RaycastHit hit;

				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Ray ray = Camera.main.ScreenPointToRay(mousePosition);

				if (!Physics.Raycast(ray, out hit)) return;
				target = hit.transform;
			}

			((IPuzzle)this).ClickedInteractable(target);
			((IPuzzle)this).CheckPuzzleAnswer();
		}

		protected virtual Transform GetActiveSelectionForGamepad() { return null; }

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
