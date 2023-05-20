using UnityEngine;
using System.Collections;

using Helper;
using Personal.UI.Option;
using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.FSM.Character;
using Personal.FSM;
using System;

namespace Personal.Manager
{
	public class UIManager : GameInitializeSingleton<UIManager>
	{
		[SerializeField] OptionHandlerUI optionUI = null;

		public OptionHandlerUI OptionUI { get => optionUI; }

		InputReader inputReader;

		// TODO : Should handle all the other UI here as well.

		protected override async UniTask Awake()
		{
			await base.Awake();
			await UniTask.Yield(PlayerLoopTiming.LastInitialization);

			Initalize();
		}

		void Initalize()
		{
			inputReader = InputManager.Instance.InputReader;
			inputReader.OnMenuUIPressedEvent += OpenOptionMenu;

			// Option UI initialize.
			OptionUI.Initialize();
		}

		void OpenOptionMenu()
		{
			if (!OptionUI.IsOpened)
			{
				optionUI.OpenMenuTab(OptionHandlerUI.MenuTab.Graphic);
				return;
			}

			optionUI.CloseMenuTab();
		}

		void OnDestroy()
		{
			inputReader.OnMenuUIPressedEvent -= OpenOptionMenu;
		}
	}
}