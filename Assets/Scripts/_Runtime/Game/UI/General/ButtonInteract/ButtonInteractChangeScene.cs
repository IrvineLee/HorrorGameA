using UnityEngine;

using Personal.Constant;
using Personal.Manager;

namespace Personal.UI
{
	public class ButtonInteractChangeScene : ButtonInteractBase
	{
		[SerializeField] SceneType sceneType = SceneType.Main;

		public override void InitialSetup()
		{
			base.InitialSetup();
			button.onClick.AddListener(ChangeScene);
		}

		void ChangeScene()
		{
			GameSceneManager.Instance.ChangeLevel(sceneType.ToString(), isIgnoreTimescale: false);
			InputManager.Instance.DisableAllActionMap();
			CursorManager.Instance.SetToMouseCursor(false);
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
