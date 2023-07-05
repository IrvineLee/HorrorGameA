using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Personal.GameState;
using Personal.Manager;
using Helper;

namespace Personal.UI
{
	[Serializable]
	public class MenuUIBase : GameInitialize
	{
		[SerializeField] UIInterfaceType uiInterfaceType = UIInterfaceType.None;
		[SerializeField] Animator windowAnimatorParent = null;

		public IDefaultHandler IDefaultHandler { get; protected set; }
		public UIInterfaceType UiInterfaceType { get => uiInterfaceType; }

		public static event Action<bool> OnPauseEvent;

		protected GameObject lastSelectedGO;

		protected CoroutineRun windowAnimatorCR = new();
		protected int animIsExpand;

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
		public virtual void InitialSetup()
		{
			if (!windowAnimatorParent) return;

			animIsExpand = Animator.StringToHash("IsExpand");
		}

		public virtual void OpenWindow()
		{
			if (!windowAnimatorCR.IsDone) return;

			UIManager.Instance.WindowStack.Push(this);
			EnableGO(true, false);

			OnPauseEvent?.Invoke(true);
		}

		/// <summary>
		/// Close the window. Returns true if it's the final window.
		/// </summary>
		/// <returns></returns>
		public virtual void CloseWindow(bool isInstant = false)
		{
			if (!windowAnimatorCR.IsDone && !isInstant) return;

			UIManager.Instance.WindowStack.Pop();
			EnableGO(false, isInstant);

			if (!UIManager.Instance.IsWindowStackEmpty) return;

			InputManager.Instance.SetToDefaultActionMap();
			OnPauseEvent?.Invoke(false);
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
		void EnableGO(bool isFlag, bool isInstant)
		{
			if (!isInstant && windowAnimatorParent)
			{
				HandleAnimator(isFlag);
				return;
			}

			if (windowAnimatorParent)
			{
				windowAnimatorParent.gameObject.SetActive(false);
				return;
			}
			gameObject.SetActive(isFlag);
		}

		/// <summary>
		/// Handle animator's animation.
		/// </summary>
		/// <param name="isFlag"></param>
		void HandleAnimator(bool isFlag)
		{
			if (isFlag)
			{
				windowAnimatorParent.gameObject.SetActive(true);
				windowAnimatorParent.SetBool(animIsExpand, true);
				return;
			}

			windowAnimatorParent.SetBool(animIsExpand, false);
			windowAnimatorCR = CoroutineHelper.WaitUntilCurrentAnimationEnds(windowAnimatorParent, () =>
			{
				windowAnimatorParent.gameObject.SetActive(false);
			}, true);
		}
	}
}
