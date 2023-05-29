using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.UI;
using Personal.UI.Option;
using Personal.UI.Dialog;

namespace Personal.Manager
{
	public class UIManager : GameInitializeSingleton<UIManager>
	{
		[SerializeField] OptionHandlerUI optionUI = null;
		[SerializeField] DialogBoxHandlerUI dialogBoxUI = null;
		[SerializeField] CinematicBars cinematicBars = null;
		[SerializeField] FooterIconDisplay footerIconDisplay = null;

		public OptionHandlerUI OptionUI { get => optionUI; }
		public DialogBoxHandlerUI DialogBoxUI { get => dialogBoxUI; }
		public CinematicBars CinematicBars { get => cinematicBars; }
		public FooterIconDisplay FooterIconDisplay { get => footerIconDisplay; }

		// TODO : Should handle all the other UI here as well.

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
			dialogBoxUI.Initialize();
		}
	}
}