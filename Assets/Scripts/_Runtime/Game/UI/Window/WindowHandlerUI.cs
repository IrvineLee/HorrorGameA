using System.Collections.Generic;
using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using static Personal.UI.Window.WindowEnum;

namespace Personal.UI.Window
{
	public class WindowHandlerUI : MonoBehaviour
	{
		[Serializable]
		public class ButtonInfo
		{
			[SerializeField] ButtonDisplayType buttonType = ButtonDisplayType.One_Ok;
			[SerializeField] WindowButtonPress buttonPress = null;

			public ButtonDisplayType ButtonType { get => buttonType; }
			public WindowButtonPress ButtonPress { get => buttonPress; }
		}

		Dictionary<WindowUIType, WindowMenuUI> windowUIDictionary = new();

		/// <summary>
		/// Open window with certain parameters.
		/// </summary>
		public async UniTask OpenWindow(WindowUIType windowUIType, Action action01 = default, Action action02 = default, Action action03 = default)
		{
			var entity = MasterDataManager.Instance.WindowUI.Get(windowUIType);

			// Spawn the display window if it hasn't been created.
			if (!windowUIDictionary.TryGetValue(windowUIType, out WindowMenuUI windowMenuUI))
			{
				GameObject go = await AddressableHelper.Spawn(entity.windowDisplayType.GetStringValue(), Vector3.zero, transform);
				windowMenuUI = go.GetComponentInChildren<WindowMenuUI>();

				windowMenuUI.InitialSetup();
				windowMenuUI.SetSize(new Vector2(entity.widthRatio * Screen.width, entity.heightRatio * Screen.height));

				windowUIDictionary.Add(windowUIType, windowMenuUI);
				SetWindowButton(windowMenuUI, entity, action01, action02, action03);

				return;
			}

			ActivateWindow(windowMenuUI);
		}

		/// <summary>
		/// Attach buttons to the window.
		/// </summary>
		async void SetWindowButton(WindowMenuUI windowMenuUI, WindowUIEntity entity, Action action01, Action action02, Action action03)
		{
			ButtonDisplayType buttonDisplayType = entity.buttonDisplayType;
			string title = entity.title_EN;
			string description = entity.description_EN;

			GameObject go = await AddressableHelper.Spawn(entity.buttonDisplayType.GetStringValue(), Vector3.zero, windowMenuUI.transform);
			WindowButtonPress buttonPress = go.GetComponentInChildren<WindowButtonPress>();

			if (buttonDisplayType == ButtonDisplayType.One_Ok)
			{
				windowMenuUI.SetOneButtonOk(buttonPress, title, description, action01);
			}
			else if (buttonDisplayType == ButtonDisplayType.Two_YesNo)
			{
				windowMenuUI.SetTwoButtonYesNo(buttonPress, title, description, action01, action02);
			}
			else if (buttonDisplayType == ButtonDisplayType.Three)
			{
				windowMenuUI.SetThreeButton(buttonPress, title, description, action01, action02, action03);
			}

			ActivateWindow(windowMenuUI);
		}

		// Push it to the stack and enable it.
		void ActivateWindow(WindowMenuUI windowMenuUI)
		{
			// Don't do anything if the window is already opened.
			if (windowMenuUI.IsWindowOpened) return;

			windowMenuUI.OpenWindow();
		}
	}
}
