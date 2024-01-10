using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Helper;
using Personal.GameState;
using Personal.Spawner;
using Personal.Character.Player;
using Personal.InteractiveObject;
using Personal.Transition;
using Personal.Character;
using Personal.Dialogue;
using Personal.Quest;
using Personal.InputProcessing;

namespace Personal.Manager
{
	/// <summary>
	/// Handles everything that are happening within the stage.
	/// </summary>
	public class StageManager : GameInitializeSingleton<StageManager>
	{
		public MasterDataManager MasterData { get => MasterDataManager.Instance; }

		public CameraHandler CameraHandler { get; private set; }
		public PlayerController PlayerController { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }
		public DialogueController DialogueController { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }
		public bool IsBusy { get => TransitionManager.Instance.IsTransitioning; }
		public List<KeyEventType> keyEventCompletedList { get; private set; } = new();

		public static event Action<KeyEventType> OnKeyEventCompleted;

		protected override void Initialize()
		{
			CameraHandler = Camera.main.GetComponentInChildren<CameraHandler>();
			DialogueController = DialogueManager.Instance.GetComponentInChildren<DialogueController>();
		}

		protected override async UniTask OnEarlyMainSceneAsync()
		{
			// If there is a main scene handler, let that script handle it.
			var mainSceneHandler = FindObjectOfType<MainSceneHandler>();
			if (mainSceneHandler) return;

			// Deactivate the black screen.
			UIManager.Instance.ToolsUI.BlackScreen(false);

			// Make the player not able to do anything.
			InputManager.Instance.DisableAllActionMap();
			await UniTask.WaitUntil(() => !IsBusy, cancellationToken: this.GetCancellationTokenOnDestroy());
			InputManager.Instance.EnableActionMap(ActionMapType.Player);
		}

		public void SetMainCameraTransform(Transform target)
		{
			CameraHandler?.SetPosAndRot(target);
		}

		public void RegisterPlayer(PlayerController pc)
		{
			PlayerController = pc;
		}

		public void RegisterCashierNPCSpawner(CashierNPCSpawner cashierNPCSpawner)
		{
			CashierNPCSpawner = cashierNPCSpawner;
		}

		public void NextDay()
		{
			DayIndex++;
			CashierInteractionIndex = 0;
		}

		public void NextInteraction()
		{
			CashierInteractionIndex++;
		}

		public void SetInteraction(int index)
		{
			CashierInteractionIndex = index;
		}

		public void RegisterKeyEvent(KeyEventType keyEventType)
		{
			if (keyEventCompletedList.Contains(keyEventType)) return;

			keyEventCompletedList.Add(keyEventType);
			OnKeyEventCompleted?.Invoke(keyEventType);
		}

		public async UniTask GetReward(List<InteractableObject> rewardInteractableObjectList)
		{
			if (rewardInteractableObjectList.Count <= 0) return;

			// Enable the gameobjects so they can be initialized first(for those that wasn't awake at runtime).
			foreach (var reward in rewardInteractableObjectList)
			{
				reward.gameObject.SetActive(true);
			}

			await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

			foreach (var reward in rewardInteractableObjectList)
			{
				var interactable = (InteractablePickupable)reward;

				if (reward.GetType() == typeof(InteractablePickupable))
				{
					PlayerController.Inventory.AddItem(interactable);
				}

				// Update quest after adding items to inventory.
				QuestTypeSet questTypeSet = interactable.GetComponentInChildren<QuestTypeSet>();
				questTypeSet?.TryUpdateData();
			}
		}

		public void ResetStage()
		{
			// Prevent virtual camera from updating the camera position and rotation.
			PlayerController.gameObject.SetActive(false);
			CoroutineHelper.WaitEndOfFrame(CameraHandler.ResetPosAndRot);

			PlayerController.Inventory.ResetInventory();
		}
	}
}