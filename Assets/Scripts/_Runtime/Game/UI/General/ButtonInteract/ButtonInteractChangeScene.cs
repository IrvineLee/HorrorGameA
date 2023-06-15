using UnityEngine;

using Personal.Constant;
using Personal.Manager;
using Helper;

namespace Personal.UI
{
	public class ButtonInteractChangeScene : ButtonInteractSet
	{
		[SerializeField] SceneType sceneType = SceneType.Main;

		public override void Initialize()
		{
			button.onClick.AddListener(ChangeScene);
		}

		void ChangeScene()
		{
			GameSceneManager.Instance.ChangeLevel(sceneType.GetStringValue());
		}

		void OnApplicationQuit()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
