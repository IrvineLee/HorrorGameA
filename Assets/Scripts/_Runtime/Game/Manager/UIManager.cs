using UnityEngine;
using System.Collections;

using Helper;
using Personal.UI.Option;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class UIManager : MonoBehaviourSingleton<UIManager>
	{
		[SerializeField] Transform playerUI = null;
		[SerializeField] OptionHandlerUI optionUI = null;

		public OptionHandlerUI OptionUI { get => optionUI; }

		// TODO : Should handle all the other UI here as well.

		async void Start()
		{
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);

			OptionHandlerUI.OnMenuOpened += OptionUI_OnMenuOpened;

			Initalize();
		}

		void OptionUI_OnMenuOpened(bool isFlag)
		{
			playerUI.gameObject.SetActive(!isFlag);
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