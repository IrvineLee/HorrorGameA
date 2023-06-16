using UnityEngine;

using Personal.GameState;
using Personal.Spawner;
using Personal.Character.Player;
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

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }

		CoroutineRun beginCR = new CoroutineRun();

		protected override void Initialize()
		{
			// The camera in the Title scene.
			MainCamera = Camera.main;
		}

		protected override void OnEarlyMainScene()
		{
			InputManager.Instance.DisableAllActionMap();

			beginCR?.StopCoroutine();
			beginCR = CoroutineHelper.WaitFor(1f, () => InputManager.Instance.SetToDefaultActionMap());
		}

		public void Load(PlayerController pc)
		{
			PlayerController = pc;

			// Set the camera in Main scene.
			MainCamera = Camera.main;
			CinemachineBrain = MainCamera.GetComponentInChildren<CinemachineBrain>();
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
	}
}

