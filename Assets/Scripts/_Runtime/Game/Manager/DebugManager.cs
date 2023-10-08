using UnityEngine;

using Personal.GameState;
using Personal.InteractiveObject;
using Personal.UI.Debug;

namespace Personal.Manager
{
	public class DebugManager : GameInitializeSingleton<DebugManager>
	{
		[SerializeField] DebugHandlerUI debugHandlerUI = null;

		protected override void Initialize()
		{
			if (!Debug.isDebugBuild)
			{
				gameObject.SetActive(false);
			}

			debugHandlerUI.InitialSetup();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.F9))
			{
				SaveManager.Instance.SaveSlotData();
			}
			else if (Input.GetKeyDown(KeyCode.F12))
			{
				SaveManager.Instance.LoadSlotData();
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				StageManager.Instance.CashierNPCSpawner.SpawnCashierActor();
			}
			else if (Input.GetKeyDown(KeyCode.LeftAlt))
			{
				FindObjectOfType<PhoneHandler>()?.Ring();
			}
			else if (Input.GetKeyDown(KeyCode.RightControl))
			{
				AchievementManager.Instance.Unlock(Achievement.AchievementType.Clear_Game_Once);
			}
			else if (Input.GetKeyDown(KeyCode.RightShift))
			{
				AchievementManager.Instance.ResetAll();
			}
		}

		public void OpenDebugPanel()
		{
			debugHandlerUI?.OpenWindow();
		}

		public void CloseDebugPanel()
		{
			debugHandlerUI?.CloseWindow();
		}

		public void DeleteProfileData()
		{
			SaveManager.Instance.DeleteProfileData();
		}

		public void DeleteSlotData()
		{
			SaveManager.Instance.DeleteSlotData();
		}
	}
}

