using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.InteractiveObject;
using Personal.UI.Debugging;
using Personal.UI;

namespace Personal.Manager
{
	public class DebugManager : GameInitializeSingleton<DebugManager>
	{
		[SerializeField] Button openButton = null;
		[SerializeField] Button closeButton = null;
		[SerializeField] DebugHandlerUI debugHandlerUI = null;

		protected override void Initialize()
		{
			if (!Debug.isDebugBuild)
			{
				gameObject.SetActive(false);
			}

			debugHandlerUI.InitialSetup();
			closeButton.onClick.AddListener(() => CursorManager.Instance.HandleMouse(false));
		}

		void Update()
		{
			openButton.gameObject.SetActive(!UIManager.IsWindowStackEmpty);

			if (Input.GetKeyDown(KeyCode.F9))
			{
				if (!GameSceneManager.Instance.IsMainScene()) return;
				SaveManager.Instance.SaveSlotData();
			}
			else if (Input.GetKeyDown(KeyCode.F12))
			{
				if (!GameSceneManager.Instance.IsMainScene()) return;
				SaveManager.Instance.LoadSlotData();
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				StageManager.Instance.CashierNPCSpawner.SpawnCashierActor().Forget();
			}
			else if (Input.GetKeyDown(KeyCode.LeftAlt))
			{
				//FindObjectOfType<PhoneHandler>()?.Ring();
			}
			else if (Input.GetKeyDown(KeyCode.RightControl))
			{
				//AchievementManager.Instance.Unlock(Achievement.AchievementType.Clear_Game_Once);
			}
			else if (Input.GetKeyDown(KeyCode.RightShift))
			{
				//AchievementManager.Instance.ResetAll();
			}
		}

		public void OpenDebugPanel()
		{
			debugHandlerUI?.OpenWindow();
			UISelectable.CurrentAppearSelected();
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

		void OnDestroy()
		{
			closeButton.onClick.RemoveAllListeners();
		}
	}
}

