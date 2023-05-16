using UnityEngine;
using System.Collections;

using Helper;
using Personal.UI.Option;
using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.Manager
{
	public class UIManager : GameInitializeSingleton<UIManager>
	{
		[SerializeField] Transform crosshairUI = null;
		[SerializeField] OptionHandlerUI optionUI = null;

		public OptionHandlerUI OptionUI { get => optionUI; }

		// TODO : Should handle all the other UI here as well.

		protected override async UniTask Awake()
		{
			await base.Awake();

			OptionHandlerUI.OnMenuOpened += OptionUI_OnMenuOpened;

			Initalize();
		}

		void OptionUI_OnMenuOpened(bool isFlag)
		{
			crosshairUI.gameObject.SetActive(!isFlag);
		}

		void Initalize()
		{
			// Option UI initialize.
			OptionUI.Initialize();
		}

		void OnDestroy()
		{
			OptionHandlerUI.OnMenuOpened -= OptionUI_OnMenuOpened;
		}
	}
}