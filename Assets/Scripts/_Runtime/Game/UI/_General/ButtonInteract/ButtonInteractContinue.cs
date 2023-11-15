using UnityEngine;

using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	/// <summary>
	/// This only handles 1 save slot. When you click the button, it loads from that slot.
	/// </summary>
	public class ButtonInteractContinue : ButtonInteractBase
	{
		int saveSlot = -1;

		public override void InitialSetup()
		{
			base.InitialSetup();
			button.onClick.AddListener(LoadGame);

			// If you don't have latest save slot, remove the continue button.
			saveSlot = GameStateBehaviour.Instance.SaveProfile.LatestSaveSlot;
			if (saveSlot >= 0) gameObject.SetActive(true);
		}

		void LoadGame()
		{
			SaveManager.Instance.LoadSlotData(saveSlot);
			GameSceneManager.Instance.ChangeLevel(GameStateBehaviour.Instance.SaveObject.PlayerSavedData.SceneName);
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
