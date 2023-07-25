using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Spawner;
using Personal.Character.Player;
using Personal.InteractiveObject;
using Personal.Transition;
using Cinemachine;
using Helper;

namespace Personal.Manager
{
	/// <summary>
	/// Handles all the thing that are happening within the stage.
	/// </summary>
	public class StageManager : GameInitializeSingleton<StageManager>
	{
		public MasterDataManager MasterData { get => MasterDataManager.Instance; }

		public Camera MainCamera { get; private set; }
		public CinemachineBrain CinemachineBrain { get; private set; }
		public PlayerController PlayerController { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }
		public PhoneHandler PhoneHandler { get; private set; }
		public DialogueSystemController DialogueSystemController { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }
		public bool IsBusy { get => TransitionManager.Instance.IsTransitioning; }

		protected override void Initialize()
		{
			MainCamera = Camera.main;
			DialogueSystemController = DialogueManager.Instance.GetComponentInChildren<DialogueSystemController>();
		}

		public async void SetMainCameraTransform(Transform target)
		{
			await UniTask.WaitUntil(() => MainCamera);

			// Set camera transform to MainCamera.
			MainCamera.transform.position = target.transform.position;
			MainCamera.transform.rotation = target.transform.rotation;

			CinemachineBrain = MainCamera.GetComponentInChildren<CinemachineBrain>();
		}

		public void RegisterPlayer(PlayerController pc)
		{
			PlayerController = pc;
		}

		public void RegisterCashierNPCSpawner(CashierNPCSpawner cashierNPCSpawner)
		{
			CashierNPCSpawner = cashierNPCSpawner;
		}

		public void RegisterPhoneHandler(PhoneHandler phoneHandler)
		{
			PhoneHandler = phoneHandler;
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

			CoroutineHelper.WaitEndOfFrame(() =>
			{
				MainCamera.transform.localPosition = Vector3.zero;
				MainCamera.transform.localRotation = Quaternion.identity;
			});

			PlayerController.Inventory.ResetInventoryUI();
		}
	}
}

