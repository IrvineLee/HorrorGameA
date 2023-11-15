using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Save;
using static Personal.Manager.InputManager;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Personal.Puzzle
{
	public class PuzzleController : MonoBehaviour, IDataPersistence
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
			if (!InputManager.Instance.GetButtonPush(ButtonPush.Submit) &&
				!InputManager.Instance.GetButtonPush(ButtonPush.Cancel)) return;

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

			if (InputManager.Instance.GetButtonPush(ButtonPush.Submit))
			{
				((IPuzzle)this).ClickedInteractable(target);
				((IPuzzle)this).CheckPuzzleAnswer();
			}
			else
			{
				((IPuzzle)this).CancelledInteractable(target);
			}
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
		}

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

#if UNITY_EDITOR
		void OnValidate()
		{
			if (PrefabUtility.IsPartOfPrefabAsset(gameObject)) return;
			if (string.IsNullOrEmpty(id) || gameObject.name.IsDuplicatedGameObject()) GenerateGUID();
		}
#endif
	}
}
