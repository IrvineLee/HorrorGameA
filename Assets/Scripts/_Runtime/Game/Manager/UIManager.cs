using UnityEngine;

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
		[SerializeField] DialogBoxHandlerUI dialogBoxUI = null;
		[SerializeField] CinematicBars cinematicBars = null;
		[SerializeField] FooterIconDisplay footerIconDisplay = null;

		public UIInterfaceType ActiveInterfaceType { get => activeInterfaceType; }
		public OptionHandlerUI OptionUI { get => optionUI; }
		public DialogBoxHandlerUI DialogBoxUI { get => dialogBoxUI; }
		public CinematicBars CinematicBars { get => cinematicBars; }
		public FooterIconDisplay FooterIconDisplay { get => footerIconDisplay; }

		public InventoryHandlerUI InventoryUI { get; private set; }

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

		/// <summary>
		/// This is used to register other ui elements from different canvases(camera/world).
		/// It's easier for those canvases to remain in scene for editing purposes.
		/// </summary>
		/// <param name="uiInterfaceType"></param>
		/// <param name="go"></param>
		public void RegisterInterfaceUI(UIInterfaceType uiInterfaceType, GameObject go)
		{
			if (uiInterfaceType == UIInterfaceType.Inventory)
			{
				InventoryUI = go.GetComponentInChildren<InventoryHandlerUI>();
			}
		}

		public void SetActiveInterfaceType(UIInterfaceType activeInterfaceType) { this.activeInterfaceType = activeInterfaceType; }
	}
}