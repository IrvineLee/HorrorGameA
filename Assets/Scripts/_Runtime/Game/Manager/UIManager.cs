using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.UI.Option;
using Personal.GameState;
using Personal.UI;

namespace Personal.Manager
{
	public class UIManager : GameInitializeSingleton<UIManager>
	{
		[SerializeField] OptionHandlerUI optionUI = null;
		[SerializeField] FooterIconDisplay footerIconDisplay = null;

		public OptionHandlerUI OptionUI { get => optionUI; }
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
		}
	}
}