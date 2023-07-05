using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Spawner;
using Personal.Character.Player;
using Personal.InteractiveObject;
using Personal.InputProcessing;
using Cinemachine;

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

		protected override void Initialize()
		{
			// The camera in the Title scene.
			MainCamera = Camera.main;

			DialogueSystemController = FindObjectOfType<DialogueSystemController>();
		}

		protected override void OnMainScene()
		{
			InputManager.Instance.EnableActionMap(ActionMapType.Player);
		}

		public void RegisterCamera(Camera cam)
		{
			// Set the camera in Main scene.
			MainCamera = cam;
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
	}
}

