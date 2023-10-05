using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.GameState;
using Personal.UI;
using Personal.UI.Option;
using Personal.UI.Window;

namespace Personal.Manager
{
	public class UIManager : GameInitializeSingleton<UIManager>
	{
		[SerializeField] PauseHandlerUI pauseMenuUI = null;
		[SerializeField] OptionHandlerUI optionUI = null;
		[SerializeField] InventoryHandlerUI inventoryUI = null;
		[SerializeField] ToolsHandlerUI toolsHandlerUI = null;
		[SerializeField] WindowHandlerUI windowHandlerUI = null;
		[SerializeField] MainDisplayHandlerUI mainDisplayHandlerUI = null;
		[SerializeField] FooterIconDisplay footerIconDisplay = null;

		[ShowInInspector]
		public UIInterfaceType ActiveInterfaceType { get => !IsWindowStackEmpty ? WindowStack.Peek().UiInterfaceType : UIInterfaceType.None; }
		public PauseHandlerUI PauseUI { get => pauseMenuUI; }
		public OptionHandlerUI OptionUI { get => optionUI; }
		public InventoryHandlerUI InventoryUI { get => inventoryUI; }
		public ToolsHandlerUI ToolsUI { get => toolsHandlerUI; }
		public WindowHandlerUI WindowUI { get => windowHandlerUI; }
		public MainDisplayHandlerUI MainDisplayHandlerUI { get => mainDisplayHandlerUI; }
		public FooterIconDisplay FooterIconDisplay { get => footerIconDisplay; }

		public static Stack<MenuUIBase> WindowStack { get; } = new();
		public static bool IsWindowStackEmpty { get => WindowStack.Count <= 0; }

		protected override async void Initialize()
		{
			await UniTask.NextFrame();

			pauseMenuUI.InitialSetup();
			optionUI.InitialSetup();
		}

		protected override void OnMainScene()
		{
			inventoryUI.InitialSetup();
		}

		public void CloseWindowStack()
		{
			// Dialogue will close by itself and pop from the stack.
			if (ActiveInterfaceType == UIInterfaceType.Dialogue) return;

			if (IsWindowStackEmpty) return;
			WindowStack.Peek().CloseWindow();
		}

		/// <summary>
		/// Close all window stack. 
		/// Typically used when changing scene while windows are opened.
		/// </summary>
		public void CloseAllWindowAndUIInterfaceStack(bool isDisableAllActionMap = true)
		{
			int count = WindowStack.Count;
			for (int i = 0; i < count; i++)
			{
				WindowStack.Peek().CloseWindow(true);
			}
			WindowStack.Clear();

			// Since closing the window will automatically reset it back to default action map, make sure it's disabled when needed.
			if (isDisableAllActionMap) InputManager.Instance.DisableAllActionMap();
		}
	}
}