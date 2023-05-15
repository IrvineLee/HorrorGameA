using UnityEngine;
using Personal.Manager;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public abstract class MenuUIBase : MonoBehaviour
	{
		UniTask uniTask = new UniTask();

		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public virtual UniTask Initialize() { return uniTask; }

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
