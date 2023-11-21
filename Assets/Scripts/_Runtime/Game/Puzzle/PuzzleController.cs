using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Save;
using Personal.InputProcessing;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Personal.Puzzle
{
	public class PuzzleController : ControlInputBase, IDataPersistence
	{
		public enum PuzzleState
		{
			None = 0,
			Completed,
			Failed,
		}

		// This is a unique ID for saving/loading objects in scene.
		[SerializeField] [ReadOnly] protected string id;

		[SerializeField] InteractableEventBegin interactableEventBegin = null;
		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		protected PuzzleState puzzleState = PuzzleState.None;
		protected PuzzleGamepadMovement puzzleGamepadMovement;

		protected CoroutineRun slideCR = new CoroutineRun();

		PhysicsRaycaster physicsRaycaster;

		protected override void Initialize()
		{
			puzzleGamepadMovement = GetComponentInChildren<PuzzleGamepadMovement>();
			physicsRaycaster = StageManager.Instance.CameraHandler.PhysicsRaycaster;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			InputManager.OnDeviceIconChanged += HandlePhysicsRaycaster;
		}

		protected override void ButtonSouth_Submit()
		{
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

		protected override void ButtonEast_Cancel()
		{
			((IPuzzle)this).CancelSelected();
		}

		protected override void ButtonNorth()
		{
			if (!slideCR.IsDone) return;

			((IPuzzle)this).ResetToDefault();
		}

		protected override void R3()
		{
			if (!slideCR.IsDone) return;

			((IPuzzle)this).AutoComplete();

			if (puzzleState != PuzzleState.Completed) GetReward();
			puzzleState = PuzzleState.Completed;
		}

		protected virtual Transform GetActiveSelectionForGamepad() { return null; }

		protected virtual void GetReward()
		{
			StageManager.Instance.GetReward(rewardInteractableObjectList).Forget();
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
				physicsRaycaster.enabled = true;
				return;
			}
			physicsRaycaster.enabled = false;
		}

		void EnableGamepadMovement(bool isFlag) { puzzleGamepadMovement.enabled = isFlag; }

		protected override void OnDisable()
		{
			base.OnDisable();

			InputManager.OnDeviceIconChanged -= HandlePhysicsRaycaster;
			physicsRaycaster.enabled = false;

			ActiveControlInput = null;
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			if (puzzleState != PuzzleState.Completed) return;
			data.PuzzleDictionary.AddOrUpdateValue(id, puzzleState);
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			if (!data.PuzzleDictionary.TryGetValue(id, out PuzzleState value)) return;

			puzzleState = value;
			if (puzzleState != PuzzleState.Completed) return;

			interactableEventBegin.SetIsInteractable(false);
			GetComponent<IPuzzle>()?.AutoComplete();
		}

#if UNITY_EDITOR
		[ContextMenu("GenerateGUID")]
		void GenerateGUID()
		{
			StringHelper.GenerateNewGuid(ref id);
		}

		[ContextMenu("ResetGUID")]
		void ResetGUID()
		{
			id = "";
		}

		void OnValidate()
		{
			if (PrefabUtility.IsPartOfPrefabAsset(gameObject)) return;
			if (string.IsNullOrEmpty(id) || gameObject.name.IsDuplicatedGameObject()) GenerateGUID();
		}
#endif
	}
}
