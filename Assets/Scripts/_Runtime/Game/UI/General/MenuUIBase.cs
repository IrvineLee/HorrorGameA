using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	public class MenuUIBase : GameInitialize
	{
		public IDefaultHandler IDefaultHandler { get; protected set; }

		// This is used when the player needs to select an option from the window menu.
		// Which means the player can't press the Esc button and close the window.
		public bool IsAbleToCloseWindow { get; protected set; } = true;

		public static event Action<bool> OnPauseEvent;

		protected GameObject lastSelectedGO;

		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public virtual void InitialSetup() { }

		public virtual void OpenWindow()
		{
			gameObject.SetActive(true);
			UIManager.Instance.WindowStack.Push(this);

			OnPauseEvent?.Invoke(true);
		}

		/// <summary>
		/// Close the window. Returns true if it's the final window.
		/// </summary>
		/// <returns></returns>
		public virtual bool CloseWindow()
		{
			if (UIManager.Instance.WindowStack.Count > 1)
			{
				MenuUIBase menuUIBase = UIManager.Instance.WindowStack.Peek();
				if (!menuUIBase.IsAbleToCloseWindow) return false;

				menuUIBase.gameObject.SetActive(false);
				UIManager.Instance.WindowStack.Pop();

				return false;
			}

			gameObject.SetActive(false);
			UIManager.Instance.WindowStack.Pop();

			InputManager.Instance.SetToDefaultActionMap();

			OnPauseEvent?.Invoke(false);
			return true;
		}

		/// <summary>
		/// Call this to set data to relevant members.
		/// </summary>
		public virtual UniTask SetDataToRelevantMember() { return UniTask.CompletedTask; }

		/// <summary>
		/// Set the last selected gameobject. Typically for mouse.
		/// </summary>
		/// <param name="go"></param>
		public void SetLastSelectedGO(GameObject go) { lastSelectedGO = go; }
	}
}
