using UnityEngine;

using Personal.GameState;

namespace Personal.UI
{
	public class MenuUIBase : GameInitialize
	{
		protected GameObject lastSelectedGO;

		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public virtual void InitialSetup() { }

		public void SetLastSelectedGO(GameObject go) { lastSelectedGO = go; }
	}
}
