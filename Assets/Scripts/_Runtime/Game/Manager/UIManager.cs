using UnityEngine;
using System.Collections;

using Helper;
using Personal.UI.Option;

namespace Personal.Manager
{
	public class UIManager : MonoBehaviourSingleton<UIManager>
	{
		[SerializeField] OptionHandlerUI optionUI = null;

		public OptionHandlerUI OptionUI { get => optionUI; }

		public bool IsWindow { get => Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor; }
		public bool IsMAC { get => Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor; }

		// TODO : Should handle all the other UI here as well.

		IEnumerator Start()
		{
			yield return new WaitUntil(() => GameManager.Instance.IsLoadingOver);

			Initalize();
		}

		void Initalize()
		{
			// Option UI initialize.
			OptionUI.Initialize();
		}
	}
}

