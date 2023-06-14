using UnityEngine;

using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public class MenuUIBase : GameInitialize
	{
		public IWindowHandler IWindowHandler { get; protected set; }
		public IDefaultHandler IDefaultHandler { get; protected set; }

		protected GameObject lastSelectedGO;

		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public virtual void InitialSetup() { }

		/// <summary>
		/// Call this to set data to relevant members.
		/// </summary>
		public virtual UniTask SetDataToRelevantMember() { return new UniTask(); }

		/// <summary>
		/// Set the last selected gameobject. Typically for mouse.
		/// </summary>
		/// <param name="go"></param>
		public void SetLastSelectedGO(GameObject go) { lastSelectedGO = go; }
	}
}
