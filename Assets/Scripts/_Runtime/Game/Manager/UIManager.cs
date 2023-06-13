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
		[SerializeField] [ReadOnly] UIInterfaceType activeInterfaceType = UIInterfaceType.None;

		[SerializeField] OptionHandlerUI optionUI = null;
		[SerializeField] InventoryHandlerUI inventoryUI = null;
		[SerializeField] ToolsHandlerUI toolsHandlerUI = null;
		[SerializeField] WindowHandlerUI windowHandlerUI = null;
		[SerializeField] FooterIconDisplay footerIconDisplay = null;

		public UIInterfaceType ActiveInterfaceType { get => activeInterfaceType; }
		public OptionHandlerUI OptionUI { get => optionUI; }
		public InventoryHandlerUI InventoryUI { get => inventoryUI; }
		public ToolsHandlerUI ToolsHandlerUI { get => toolsHandlerUI; }
		public WindowHandlerUI WindowHandlerUI { get => windowHandlerUI; }
		public FooterIconDisplay FooterIconDisplay { get => footerIconDisplay; }

		protected override async UniTask Awake()
		{
			await base.Awake();
			await UniTask.Yield(PlayerLoopTiming.LastInitialization);

			Initalize();
		}

		void Initalize()
		{
			// Option UI initialize.
			optionUI.InitialSetup();
		}

		public void SetActiveInterfaceType(UIInterfaceType activeInterfaceType) { this.activeInterfaceType = activeInterfaceType; }
	}
}