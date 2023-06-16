using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Personal.GameState;
using Personal.UI;
using Personal.UI.Option;
using Personal.UI.Window;

namespace Personal.Manager
{
	public class UIManager : GameInitializeSingleton<UIManager>
	{
		[SerializeField] [ReadOnly] UIInterfaceType activeInterfaceType = UIInterfaceType.None;

		[SerializeField] PauseHandlerUI pauseMenuUI = null;
		[SerializeField] OptionHandlerUI optionUI = null;
		[SerializeField] InventoryHandlerUI inventoryUI = null;
		[SerializeField] ToolsHandlerUI toolsHandlerUI = null;
		[SerializeField] WindowHandlerUI windowHandlerUI = null;
		[SerializeField] FooterIconDisplay footerIconDisplay = null;

		public UIInterfaceType ActiveInterfaceType { get => activeInterfaceType; }
		public PauseHandlerUI PauseUI { get => pauseMenuUI; }
		public OptionHandlerUI OptionUI { get => optionUI; }
		public InventoryHandlerUI InventoryUI { get => inventoryUI; }
		public ToolsHandlerUI ToolsUI { get => toolsHandlerUI; }
		public WindowHandlerUI WindowUI { get => windowHandlerUI; }
		public FooterIconDisplay FooterIconDisplay { get => footerIconDisplay; }

		public Stack<MenuUIBase> WindowStack { get; } = new();

		protected override void Initialize()
		{
			//pauseMenuUI.InitialSetup();
			optionUI.InitialSetup();
		}

		protected override void OnPostMainScene()
		{
			inventoryUI.InitialSetup();
		}

		public void SetActiveInterfaceType(UIInterfaceType activeInterfaceType) { this.activeInterfaceType = activeInterfaceType; }
	}
}