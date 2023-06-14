using UnityEngine;

using Cysharp.Threading.Tasks;
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
		public bool IsPaused { get; private set; }

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

		protected override void OnMainScene()
		{
			UIManager.Instance.OptionUI.OnMenuOpened += Pause;
			UIManager.Instance.InventoryUI.OnMenuOpened += Pause;

			InputManager.Instance.DisableAllActionMap();

			beginCR?.StopCoroutine();
			beginCR = CoroutineHelper.WaitFor(1.05f, () => InputManager.Instance.EnableActionMap(InputProcessing.ActionMapType.Player));
		}

		public void RegisterPlayer(PlayerController pc)
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

		void Pause(bool isFlag)
		{
			Time.timeScale = isFlag ? 0 : 1;

			CursorManager.Instance.SetToMouseCursor(isFlag);
			PlayerController.FPSController.enabled = !isFlag;
		}

		void OnApplicationQuit()
		{
			UIManager.Instance.OptionUI.OnMenuOpened -= Pause;
			UIManager.Instance.InventoryUI.OnMenuOpened -= Pause;
		}
	}
}

