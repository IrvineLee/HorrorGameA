using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using Helper;
using Personal.Manager;
using Personal.Puzzle;

namespace Personal.InputProcessing
{
	public class InputMovement_PuzzleController : ControlInput, IPuzzleControlInput
	{
		[Serializable]
		class PuzzlePiece
		{
			public EventTrigger EventTrigger { get; private set; }
			public OutlinableFadeInOut OutlinableFadeInOut { get; private set; }
			public SpriteRenderer SpriteRenderer { get; private set; }

			public PuzzlePiece(EventTrigger eventTrigger, OutlinableFadeInOut outlinableFadeInOut, SpriteRenderer spriteRenderer)
			{
				EventTrigger = eventTrigger;
				OutlinableFadeInOut = outlinableFadeInOut;
				SpriteRenderer = spriteRenderer;
			}
		}

		[SerializeField] int startIndex = 0;

		[Tooltip("This should be the opposite rotated angle of the clickable objects parent.")]
		[SerializeField] float rotatedAngle = 0f;

		[Tooltip("This clamps the selection arc for the supposedly next object. " +
			"Ex: A value of 1 means the next object should be directly at the direction of input. " +
			"Pressing down(0, -1) means the x-axis of both object should be exactly the same to register as possible selection. " +
			"Value of 0 is around 180 degrees, 0.5 is around 90 degrees, 1 is around 0 degrees at input direction.")]
		[Range(0, 1)]
		[SerializeField] float selectionArcDotProduct = 0.75f;

		List<PuzzlePiece> puzzlePieceList = new();

		PuzzleController puzzleController;
		IPuzzle iPuzzle;

		protected override void Initialize()
		{
			puzzleController = GetComponentInChildren<PuzzleController>(true);
			iPuzzle = GetComponentInChildren<IPuzzle>(true);

			List<Transform> interactableObjectList = iPuzzle.GetInteractableObjectList();

			foreach (var interactable in interactableObjectList)
			{
				EventTrigger eventTrigger = interactable.GetComponentInChildren<EventTrigger>();
				OutlinableFadeInOut outlinableFadeInOut = interactable.GetComponentInChildren<OutlinableFadeInOut>();
				SpriteRenderer spriteRenderer = interactable.GetComponentInChildren<SpriteRenderer>();

				PuzzlePiece puzzlePiece = new(eventTrigger, outlinableFadeInOut, spriteRenderer);
				puzzlePieceList.Add(puzzlePiece);
			}
		}

		protected override void OnEnabled()
		{
			base.OnEnabled();

			CurrentActiveIndex = startIndex;
			SetSelectionActive(startIndex, true);
		}

		public override void UpdateCurrentSelection(GameObject go)
		{
			for (int i = 0; i < puzzlePieceList.Count; i++)
			{
				var puzzlePiece = puzzlePieceList[i];
				bool isFade = puzzlePiece.EventTrigger.gameObject.Equals(go);

				SetSelectionActive(i, isFade);
				if (isFade) CurrentActiveIndex = i;
			}
		}

		protected override void HandleMovement(Vector2 move, Action endConfirmButtonAction = default)
		{
			int nextIndex = -1;
			float shortestSqrMagnitude = float.MaxValue;
			Vector3 currentPosition = puzzlePieceList[CurrentActiveIndex].EventTrigger.transform.localPosition;

			for (int i = 0; i < puzzlePieceList.Count; i++)
			{
				Vector3 triggerPosition = puzzlePieceList[i].EventTrigger.transform.localPosition;
				if (CurrentActiveIndex == i) continue;

				if (!IsMovementPossible(move, currentPosition, triggerPosition)) continue;
				if (!IsDistanceShorterThan(ref shortestSqrMagnitude, triggerPosition, currentPosition)) continue;

				nextIndex = i;
			}

			if (nextIndex < 0) return;

			SetSelectionActive(CurrentActiveIndex, false);
			SetSelectionActive(nextIndex, true);

			CurrentActiveIndex = nextIndex;
		}

		/// <summary>
		/// Movement are free to go anywhere. Analog input will have a slow start, so just put it to the max value.
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		protected override Vector2 GetHorizontalVerticalMovement(Vector2 move)
		{
			if (move.x > 0.1f)
			{
				move.x = move.x > 0 ? 1 : -1;
			}
			if (move.y > 0.1f)
			{
				move.y = move.y > 0 ? 1 : -1;
			}
			return move;
		}

		/// <summary>
		/// Check to see whether movement selection is possible.
		/// </summary>
		/// <param name="move"></param>
		/// <param name="currentPosition"></param>
		/// <param name="triggerPosition"></param>
		/// <returns></returns>
		bool IsMovementPossible(Vector2 move, Vector3 currentPosition, Vector3 triggerPosition)
		{
			// Normalize the direction of from target -> to target.
			Vector3 direction = Vector3.Normalize(triggerPosition - currentPosition);

			// Change the input direction into vector3 and normalize it.
			Quaternion quaternion = Quaternion.Euler(0, 0, rotatedAngle);
			Vector3 adjustedPosition = quaternion * move;
			Vector3 activeFaceDirection = adjustedPosition.normalized;

			// Get the dot product and checks whether it's between the selection arc.
			float dotProduct = Vector3.Dot(activeFaceDirection, direction);
			if (dotProduct < selectionArcDotProduct) return false;

			return true;
		}

		/// <summary>
		/// Save the shortest distance(squared magnitude).
		/// </summary>
		/// <param name="shortestSqrMagnitude"></param>
		/// <param name="target1"></param>
		/// <param name="target2"></param>
		/// <returns></returns>
		bool IsDistanceShorterThan(ref float shortestSqrMagnitude, Vector3 target1, Vector3 target2)
		{
			Vector3 distance = target1 - target2;
			if (shortestSqrMagnitude > distance.sqrMagnitude)
			{
				shortestSqrMagnitude = distance.sqrMagnitude;
				return true;
			}
			return false;
		}

		void SetSelectionActive(int index, bool isFlag)
		{
			puzzlePieceList[index].OutlinableFadeInOut.StartFade(isFlag);
		}

		void IPuzzleControlInput.Submit()
		{
			if (!puzzleController.IsBusy) return;

			// Check puzzle click.
			Transform target = puzzleController.GetActiveSelectionForGamepad();
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				RaycastHit hit;

				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Ray ray = Camera.main.ScreenPointToRay(mousePosition);

				if (!Physics.Raycast(ray, out hit)) return;
				target = hit.transform;
			}

			iPuzzle.ClickedInteractable(target);
			iPuzzle.CheckPuzzleAnswer();
		}

		void IPuzzleControlInput.Cancel()
		{
			if (!puzzleController.IsBusy) return;

			iPuzzle.CancelSelected();
		}

		void IPuzzleControlInput.Reset()
		{
			if (!puzzleController.IsBusy) return;

			iPuzzle.ResetToDefault();
		}

		void IPuzzleControlInput.AutoComplete()
		{
			if (!puzzleController.IsBusy) return;

			iPuzzle.AutoComplete();
		}

		protected override void OnDisabled()
		{
			base.OnDisabled();
			SetSelectionActive(CurrentActiveIndex, false);
		}
	}
}