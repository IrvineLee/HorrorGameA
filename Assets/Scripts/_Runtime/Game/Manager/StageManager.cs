using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Spawner;
using Personal.Character.Player;
using Cinemachine;

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

		protected override async UniTask Awake()
		{
			await base.Awake();

			// Starting in main scene.
			if (GameSceneManager.Instance.IsMainScene())
			{
				InitialSetup();
				return;
			}

			MainCamera = Camera.main;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void InitialSetup()
		{
			MainCamera = Camera.main;
			CinemachineBrain = MainCamera.GetComponentInChildren<CinemachineBrain>();
			PlayerController = FindObjectOfType<PlayerController>();

			UIManager.Instance.OptionUI.OnMenuOpened += Pause;
			UIManager.Instance.InventoryUI.OnMenuOpened += Pause;
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

		/// <summary>
		/// Initialize the value when entering Main scene.
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="mode"></param>
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (string.Equals(name, SceneName.Main)) return;

			InitialSetup();
		}

		void OnApplicationQuit()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;

			UIManager.Instance.OptionUI.OnMenuOpened -= Pause;
			UIManager.Instance.InventoryUI.OnMenuOpened -= Pause;
		}
	}
}

