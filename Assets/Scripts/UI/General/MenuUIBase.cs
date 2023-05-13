using UnityEngine;
using Personal.Manager;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public abstract class MenuUIBase : MonoBehaviour
	{
		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask Initialize() { await UniTask.Delay(0); }

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
