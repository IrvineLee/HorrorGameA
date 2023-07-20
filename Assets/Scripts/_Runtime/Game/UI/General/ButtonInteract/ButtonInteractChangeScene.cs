using UnityEngine;

using Personal.Constant;
using Personal.Manager;
using Helper;

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
			GameSceneManager.Instance.ChangeLevel(sceneType.GetStringValue(), isIgnoreTimescale: false);
			InputManager.Instance.DisableAllActionMap();
			CursorManager.Instance.SetToMouseCursor(false);
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
