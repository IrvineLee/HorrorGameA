using UnityEngine;
using System.Collections;

using Helper;
using Personal.UI.Option;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class UIManager : MonoBehaviourSingleton<UIManager>
	{
		[SerializeField] OptionHandlerUI optionUI = null;

		public OptionHandlerUI OptionUI { get => optionUI; }

		// TODO : Should handle all the other UI here as well.

		async void Start()
		{
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);

			Initalize();
		}

		void Initalize()
		{
			// Option UI initialize.
			OptionUI.Initialize();
		}
	}
}

