using UnityEngine;
using System.Collections;

using Personal.Manager;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public abstract class GenericMenuUI : MonoBehaviour
	{
		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask Initialize()
		{
			// To make sure the loading is over.
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);
		}

		/// <summary>
		/// Pressing the OK button.
		/// </summary>
		public abstract void Save_Inspector();

		/// <summary>
		/// Closing the menu.
		/// </summary>
		public abstract void Cancel_Inspector();
	}
}
