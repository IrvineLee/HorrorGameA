using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Helper;
using Personal.GameState;
using Personal.Manager;
using Personal.UI.Window;

namespace Personal.UI
{
	[Serializable]
	public class MenuUIBase : GameInitialize
	{
		[SerializeField] protected UIInterfaceType uiInterfaceType = UIInterfaceType.None;
		[SerializeField] protected WindowSelectionUIAnimator windowUIAnimator = null;

		public IDefaultHandler IDefaultHandler { get; protected set; }
		public UIInterfaceType UiInterfaceType { get => uiInterfaceType; }
		public bool IsWindowOpened { get => windowUIAnimator ? windowUIAnimator.gameObject.activeSelf : gameObject.activeSelf; }
		protected bool IsWindowAnimationDone { get => windowUIAnimator ? windowUIAnimator.IsDone : true; }

		public static event Action<bool> OnPauseEvent;

		protected GameObject lastSelectedGO;
		protected Action disableAction;

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

		public virtual void OpenWindow()
		{
			if (!IsWindowAnimationDone) return;

			UIManager.WindowStack.Push(this);
			EnableGO(true, false);

			OnPause(true);
		}

		/// <summary>
		/// Close the window. Returns true if it's the final window.
		/// </summary>
		/// <returns></returns>
		public virtual void CloseWindow(bool isInstant = false)
		{
			if (!IsWindowAnimationDone && !isInstant) return;

			EnableGO(false, isInstant);
			CoroutineHelper.WaitNextFrame(() =>
			{
				UIManager.WindowStack.Pop();

				if (!UIManager.IsWindowStackEmpty) return;

				InputManager.Instance.SetToDefaultActionMap();
				OnPause(false);
			});
		}

		/// <summary>
		/// Set the last selected gameobject. Typically for mouse.
		/// </summary>
		/// <param name="go"></param>
		public void SetLastSelectedGO(GameObject go) { lastSelectedGO = go; }

		/// <summary>
		/// Action that will be called OnDisable.
		/// </summary>
		/// <param name="action"></param>
		public virtual void SetOnDisableAction(Action action) { disableAction = action; }

		/// <summary>
		/// OnPauseEvent for derived class.
		/// </summary>
		/// <param name="isFlag"></param>
		protected virtual void OnPause(bool isFlag)
		{
			OnPauseEvent?.Invoke(isFlag);
		}

		/// <summary>
		/// Enable/Disable the window.
		/// </summary>
		/// <param name="isFlag"></param>
		void EnableGO(bool isFlag, bool isInstant)
		{
			if (windowUIAnimator)
			{
				if (!isInstant) windowUIAnimator.Run(isFlag);
				else windowUIAnimator.gameObject.SetActive(false);

				return;
			}
			gameObject.SetActive(isFlag);
		}

		protected virtual void OnDisable()
		{
			disableAction?.Invoke();
			disableAction = default;
		}
	}
}
