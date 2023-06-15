using System;
using UnityEngine;

using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public class MenuUIBase : GameInitialize
	{
		[SerializeField] protected bool isPauseOnOpen = false;

		public IWindowHandler IWindowHandler { get; protected set; }
		public IDefaultHandler IDefaultHandler { get; protected set; }

		public static event Action<bool> OnPauseEvent;

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
		public virtual UniTask SetDataToRelevantMember() { return UniTask.CompletedTask; }

		/// <summary>
		/// Set the last selected gameobject. Typically for mouse.
		/// </summary>
		/// <param name="go"></param>
		public void SetLastSelectedGO(GameObject go) { lastSelectedGO = go; }

		/// <summary>
		/// Handle when the window opened or closed.
		/// </summary>
		/// <param name="isFlag"></param>
		protected virtual void SetupMenu(bool isFlag)
		{
			if (!isPauseOnOpen) return;

			OnPauseEvent?.Invoke(isFlag);
		}
	}
}
