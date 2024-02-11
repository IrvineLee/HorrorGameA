using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.UI.Window;
using Personal.Manager;

namespace Personal.UI
{
	[Serializable]
	public abstract class MenuUIBase : GameInitialize
	{
		[SerializeField] protected UIInterfaceType uiInterfaceType = UIInterfaceType.None;
		[SerializeField] protected WindowSelectionUIAnimator windowUIAnimator = null;

		public IDefaultHandler IDefaultHandler { get; protected set; }
		public UIInterfaceType UiInterfaceType { get => uiInterfaceType; }
		public bool IsWindowOpened { get => windowUIAnimator ? windowUIAnimator.gameObject.activeSelf : gameObject.activeSelf; }
		protected bool IsWindowAnimationDone { get => windowUIAnimator ? windowUIAnimator.IsDone : true; }

		public static event Action<bool> OnPauseEvent;

		protected GameObject lastSelectedGO;

		void Update()
		{
			if (UIManager.Instance.ActiveInterfaceType != uiInterfaceType) return;
			if (EventSystem.current.currentSelectedGameObject) return;
			if (!lastSelectedGO) return;

			EventSystem.current.SetSelectedGameObject(lastSelectedGO);
		}

		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public virtual void InitialSetup() { }

		public virtual void OpenWindow() { }
		public virtual UniTask CloseWindow(bool isInstant = false) { return UniTask.CompletedTask; }

		/// <summary>
		/// OnPauseEvent for derived class.
		/// </summary>
		/// <param name="isFlag"></param>
		protected void OnPause(bool isFlag)
		{
			OnPauseEvent?.Invoke(isFlag);
		}

		/// <summary>
		/// Set the last selected gameobject. Typically for mouse.
		/// </summary>
		/// <param name="go"></param>
		public void SetLastSelectedGO(GameObject go) { lastSelectedGO = go; }

		/// <summary>
		/// Enable/Disable the window.
		/// </summary>
		/// <param name="isFlag"></param>
		protected void EnableGO(bool isFlag, bool isInstant)
		{
			if (windowUIAnimator)
			{
				if (!isInstant) windowUIAnimator.Run(isFlag);
				else windowUIAnimator.gameObject.SetActive(false);

				return;
			}
			gameObject.SetActive(isFlag);
		}
	}
}
