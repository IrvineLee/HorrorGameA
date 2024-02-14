using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Sirenix.OdinInspector;
using Helper;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Save;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.KeyEvent;
using Personal.Interface;

namespace Personal.Puzzle
{
	public class PuzzleController : GameInitialize, IDataPersistence, IProcess
	{
		public enum PuzzleState
		{
			None = 0,
			Completed,
			Failed,
		}

		[SerializeField] [ReadOnly] string guid = Guid.NewGuid().ToString();

		[SerializeField] KeyEventType completeKeyEventType = KeyEventType.None;
		[SerializeField] InteractableObject parentInteractableObject = null;

		public bool IsBusy { get => slideCR.IsDone; }

		protected PuzzleState puzzleState;
		protected CoroutineRun slideCR = new();

		PhysicsRaycaster physicsRaycaster;
		InputMovement_PuzzleController inputMovement;

		protected override void Initialize()
		{
			physicsRaycaster = StageManager.Instance.CameraHandler.PhysicsRaycaster;
			inputMovement = GetComponentInChildren<InputMovement_PuzzleController>();
		}

		protected override void OnEnabled()
		{
			InputManager.OnDeviceIconChanged += HandlePhysicsRaycaster;
		}

		public virtual Transform GetActiveSelectionForGamepad() { return null; }

		protected virtual void EndAndGetReward()
		{
			parentInteractableObject.enabled = false;

			StageManager.Instance.RegisterKeyEvent(completeKeyEventType);
			puzzleState = PuzzleState.Completed;
		}

		void HandlePhysicsRaycaster()
		{
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				physicsRaycaster.enabled = true;
				return;
			}
			physicsRaycaster.enabled = false;
		}

		protected override void OnDisabled()
		{
			InputManager.OnDeviceIconChanged -= HandlePhysicsRaycaster;
			physicsRaycaster.enabled = false;
		}

		protected virtual void OnBegin(bool isFlag) { EnableMovement(isFlag); }

		/// <summary>
		/// Enable/disable movement.
		/// </summary>
		/// <param name="isFlag"></param>
		void EnableMovement(bool isFlag)
		{
			enabled = isFlag;
			HandlePhysicsRaycaster();

			inputMovement.enabled = isFlag;
			InputManager.Instance.EnableActionMap(isFlag ? ActionMapType.Puzzle : ActionMapType.Player);
		}

		/// <summary>
		/// Handle whether the puzzle has started.
		/// </summary>
		/// <param name="isFlag"></param>
		void IProcess.Begin(bool isFlag)
		{
			OnBegin(isFlag);
			CursorManager.Instance.HandleMouse();

			if (puzzleState == PuzzleState.Completed) return;
			puzzleState = PuzzleState.None;
		}

		/// <summary>
		/// Return if the puzzle has been completed.
		/// </summary>
		/// <returns></returns>
		bool IProcess.IsCompleted()
		{
			return puzzleState == PuzzleState.Completed;
		}

		/// <summary>
		/// Return when failed.
		/// </summary>
		/// <returns></returns>
		bool IProcess.IsFailed()
		{
			return puzzleState == PuzzleState.Failed;
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			if (puzzleState != PuzzleState.Completed) return;
			data.SceneObjectSavedData.PuzzleDictionary.AddOrUpdateValue(guid, puzzleState);
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			if (!data.SceneObjectSavedData.PuzzleDictionary.TryGetValue(guid, out PuzzleState value)) return;

			puzzleState = value;
			if (puzzleState != PuzzleState.Completed) return;

			parentInteractableObject.enabled = false;
			GetComponent<IPuzzle>()?.AutoComplete();
		}

		void OnValidate()
		{
			if (string.IsNullOrEmpty(guid) || gameObject.name.IsDuplicatedGameObject())
			{
				name = name.SearchBehindRemoveFrontOrEnd('(', true);
				guid = Guid.NewGuid().ToString();
			}
		}
	}
}
