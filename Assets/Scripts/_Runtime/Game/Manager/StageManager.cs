using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Personal.GameState;
using Personal.Spawner;
using Personal.FSM.Character;
using Personal.UI.Option;
using Cysharp.Threading.Tasks;
using PixelCrushers;

namespace Personal.Manager
{
	public class StageManager : GameInitializeSingleton<StageManager>
	{
		public bool IsPaused { get; private set; }

		public MasterDataManager MasterData { get => MasterDataManager.Instance; }

		public Camera MainCamera { get; private set; }
		public PlayerStateMachine PlayerFSM { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }

		protected override async UniTask Awake()
		{
			await base.Awake();

			MainCamera = Camera.main;
			PlayerFSM = FindObjectOfType<PlayerStateMachine>();

			InputDeviceManager.instance.ForceCursor(false);
			OptionHandlerUI.OnMenuOpened += Pause;
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

			InputDeviceManager.instance.ForceCursor(isFlag);
			PlayerFSM.FirstPersonController.StopMovement(isFlag);
		}

		void OnDestroy()
		{
			OptionHandlerUI.OnMenuOpened -= Pause;
		}
	}
}

