﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Helper;
using Personal.Spawner;
using Personal.FSM.Character;
using Cysharp.Threading.Tasks;
using PixelCrushers;
using Personal.UI.Option;

namespace Personal.Manager
{
	public class StageManager : MonoBehaviourSingleton<StageManager>
	{
		public bool IsPaused { get; private set; }

		public Camera MainCamera { get; private set; }
		public PlayerStateMachine PlayerFSM { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }

		async void Awake()
		{
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);

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

