using System;
using System.Collections.Generic;
using System.Linq;
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
using Personal.Item;
using Personal.Save;
using Personal.KeyEvent;

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
		public List<KeyEventType> KeyEventCompletedList { get => keyEventData.KeyEventList; }

		public static event Action<KeyEventType> OnKeyEventCompleted;

		KeyEventData keyEventData;

		protected override void Initialize()
		{
			CameraHandler = Camera.main.GetComponentInChildren<CameraHandler>();
			DialogueController = DialogueManager.Instance.GetComponentInChildren<DialogueController>();
		}

		protected override void OnMainScene()
		{
			keyEventData = GameStateBehaviour.Instance.SaveObject.PlayerSavedData.KeyEventData;
			HandleMainScene().Forget();
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
			if (keyEventType == KeyEventType.None) return;
			if (KeyEventCompletedList.Contains(keyEventType)) return;

			KeyEventCompletedList.Add(keyEventType);
			OnKeyEventCompleted?.Invoke(keyEventType);
		}

		/// <summary>
		/// Get the item reward.
		/// </summary>
		/// <param name="rewardItemList"></param>
		/// <returns></returns>
		public async UniTask GetReward(List<ItemData> rewardItemList)
		{
			if (rewardItemList.Count <= 0) return;

			foreach (var reward in rewardItemList)
			{
				var itemList = await AddressableHelper.SpawnMultiple(reward.ItemType.GetStringValue(), reward.Amount);
				var interactableList = itemList.Select(x => x.GetComponentInChildren<InteractableObject>()).ToList();

				foreach (var item in interactableList)
				{
					PlayerController.Inventory.AddItem((InteractablePickupable)item);

					// Update quest after adding items to inventory.
					QuestTypeSet questTypeSet = item.GetComponentInChildren<QuestTypeSet>();
					questTypeSet?.TryUpdateData();
				}
			}
		}

		public void ResetStage()
		{
			// Prevent virtual camera from updating the camera position and rotation.
			PlayerController.gameObject.SetActive(false);
			CoroutineHelper.WaitEndOfFrame(CameraHandler.ResetPosAndRot);

			PlayerController.Inventory.ResetInventory();
		}

		async UniTask HandleMainScene()
		{
			var mainSceneHandler = FindObjectOfType<MainSceneHandlerBase>();
			if (mainSceneHandler) return;

			// Deactivate the black screen.
			UIManager.Instance.ToolsUI.BlackScreen(false);

			// Make the player not able to do anything.
			InputManager.Instance.DisableAllActionMap();
			await UniTask.WaitUntil(() => !IsBusy, cancellationToken: this.GetCancellationTokenOnDestroy());
			InputManager.Instance.EnableActionMap(ActionMapType.Player);
		}
	}
}