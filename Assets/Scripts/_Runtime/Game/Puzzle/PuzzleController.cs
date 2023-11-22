using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Save;
using Personal.GameState;
using Personal.InputProcessing;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Personal.Puzzle
{
	public class PuzzleController : GameInitialize, IDataPersistence
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
			StageManager.Instance.GetReward(rewardInteractableObjectList).Forget();
			puzzleState = PuzzleState.Completed;
		}

		/// <summary>
		/// Enable/disable movement.
		/// </summary>
		/// <param name="isFlag"></param>
		protected void EnableMovement(bool isFlag)
		{
			HandlePhysicsRaycaster();

			inputMovement.enabled = isFlag;
			InputManager.Instance.EnableActionMap(isFlag ? ActionMapType.Puzzle : ActionMapType.Player);
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
