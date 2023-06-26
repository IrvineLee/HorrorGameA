﻿using UnityEngine;

using Helper;
using Personal.GameState;
using Personal.Save;
using Personal.FSM;
using Personal.FSM.Cashier;
using Personal.InputProcessing;
using Cysharp.Threading.Tasks;
using Personal.InteractiveObject;

namespace Personal.Manager
{
	public class DebugManager : GameInitializeSingleton<DebugManager>
	{
		FPSInputController FPSInputController;

		protected override void Initialize()
		{
			FPSInputController = InputManager.Instance.FPSInputController;
		}

		protected override void OnUpdate()
		{
			if (InputManager.Instance.IsInteract)
			{
				//Debug.Log("<color=red> " + InputManager.Instance.IsInteract + " </color>");
				RumbleManager.Instance.Vibrate(0.5f, 0.5f, 1f);
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				//SceneManager.Instance.ChangeLevel(1);

				//SaveManager.Instance.LoadSlotData();
				//UIManager.Instance.OptionUI.OpenMenuTab(UI.Option.OptionHandlerUI.MenuTab.Graphic);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1))
			{
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				//GameStateBehaviour.Instance.SaveObject.PlayerSavedData.IntStrDictionary = new SerializableDictionary<int, string>();
				//GameStateBehaviour.Instance.SaveObject.PlayerSavedData.IntStrDictionary.Add(0, "zero");
				//GameStateBehaviour.Instance.SaveObject.PlayerSavedData.IntStrDictionary.Add(1, "one");

				//GameStateBehaviour.Instance.SaveObject.PlayerSavedData.CharacterID = 999;
				SaveManager.Instance.SaveSlotData();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				SaveManager.Instance.LoadSlotData();
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				StageManager.Instance.CashierNPCSpawner.SpawnCashierActor();
			}
			else if (Input.GetKeyDown(KeyCode.LeftAlt))
			{
				FindObjectOfType<PhoneHandler>().Ring();
			}
		}
	}
}

