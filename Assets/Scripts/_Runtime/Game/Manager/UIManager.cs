using UnityEngine;
using UnityEngine.Rendering.Universal;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.GameState;
using Personal.UI;
using Personal.UI.Option;
using Personal.UI.Dialog;

namespace Personal.Manager
{
	public class UIManager : GameInitializeSingleton<UIManager>
	{
		[SerializeField] [ReadOnly] UIInterfaceType activeInterfaceType = UIInterfaceType.None;

		[SerializeField] OptionHandlerUI optionUI = null;
		[SerializeField] InventoryHandlerUI inventoryUI = null;
		[SerializeField] DialogBoxHandlerUI dialogBoxUI = null;
		[SerializeField] CinematicBars cinematicBars = null;
		[SerializeField] FooterIconDisplay footerIconDisplay = null;

		public UIInterfaceType ActiveInterfaceType { get => activeInterfaceType; }
		public OptionHandlerUI OptionUI { get => optionUI; }
		public InventoryHandlerUI InventoryUI { get => inventoryUI; }
		public DialogBoxHandlerUI DialogBoxUI { get => dialogBoxUI; }
		public CinematicBars CinematicBars { get => cinematicBars; }
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
			OptionUI.Initialize();
		}

		public void SetActiveInterfaceType(UIInterfaceType activeInterfaceType) { this.activeInterfaceType = activeInterfaceType; }
	}
}