using UnityEngine;
using System.Collections;

using Helper;
using Personal.UI.Option;

namespace Personal.Manager
{
	public class UIManager : MonoBehaviourSingleton<UIManager>
	{
		[SerializeField] OptionUI optionUI = null;

		// TODO : Should handle all the other UI here as well.

		IEnumerator Start()
		{
			yield return new WaitUntil(() => GameManager.Instance.IsLoadingOver);

			Initalize();
		}

		void Initalize()
		{
			// Option UI initialize.
			optionUI.Initialize();
		}
	}
}

