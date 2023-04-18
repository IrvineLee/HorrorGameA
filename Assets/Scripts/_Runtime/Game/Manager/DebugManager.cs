using UnityEngine;

using Helper;
using Personal.GameState;
using Personal.Save;

namespace Personal.Manager
{
	public class DebugManager : MonoBehaviourSingleton<DebugManager>
	{
		SaveObject saveObject;

		public void Initialize()
		{
			//saveObject = GameStateBehaviour.Instance.SaveObject;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				//SceneManager.Instance.ChangeLevel(1);

				//SaveManager.Instance.LoadSlotData();
				UIManager.Instance.OptionUI.OpenMenuTab(UI.Option.OptionHandlerUI.MenuTab.Graphic);
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
		}
	}
}

