using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Spawner;
using Personal.Character.Player;
using Personal.InteractiveObject;
using Personal.Transition;
using Personal.Character;
using Cinemachine;
using Helper;
using Personal.Dialogue;

namespace Personal.Manager
{
	/// <summary>
	/// Handles all the thing that are happening within the stage.
	/// </summary>
	public class StageManager : GameInitializeSingleton<StageManager>
	{
		public MasterDataManager MasterData { get => MasterDataManager.Instance; }

		public CameraHandler CameraHandler { get; private set; }
		public PlayerController PlayerController { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }
		public DialogueSystemController DialogueSystemController { get; private set; }
		public DialogueSetup DialogueSetup { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }
		public bool IsBusy { get => TransitionManager.Instance.IsTransitioning; }

		protected override void Initialize()
		{
			CameraHandler = Camera.main.GetComponentInChildren<CameraHandler>();
			DialogueSystemController = DialogueManager.Instance.GetComponentInChildren<DialogueSystemController>();
			DialogueSetup = DialogueManager.Instance.GetComponentInChildren<DialogueSetup>();
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

		public void ResetStage()
		{
			// Prevent virtual camera from updating the camera position and rotation.
			PlayerController.gameObject.SetActive(false);
			CoroutineHelper.WaitEndOfFrame(CameraHandler.ResetPosAndRot);

			PlayerController.Inventory.ResetInventoryUI();
		}
	}
}

