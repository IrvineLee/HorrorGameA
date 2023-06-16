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

		public static event Action<bool> OnPauseEvent;

		protected GameObject lastSelectedGO;

		public virtual void OpenWindow()
		{
			gameObject.SetActive(true);
			UIManager.Instance.WindowStack.Push(this);
		}

		public virtual void CloseWindow()
		{
			gameObject.SetActive(false);
			UIManager.Instance.WindowStack.Pop();

			InputManager.Instance.SetToDefaultActionMap();
		}

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
		/// Typically used within OpenWindow and CloseWindow.
		/// </summary>
		/// <param name="isFlag"></param>
		protected virtual void PauseEventBegin(bool isFlag)
		{
			OnPauseEvent?.Invoke(isFlag);
		}

		protected bool IsWindowStackClose()
		{
			if (UIManager.Instance.WindowStack.Count > 1)
			{
				UIManager.Instance.WindowStack.Peek().CloseWindow();
				return true;
			}
			return false;
		}
	}
}
