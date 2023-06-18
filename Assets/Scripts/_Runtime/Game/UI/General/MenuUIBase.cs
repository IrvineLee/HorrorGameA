using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	[Serializable]
	public class MenuUIBase : GameInitialize
	{
		[SerializeField] UIInterfaceType uiInterfaceType = UIInterfaceType.None;

		public IDefaultHandler IDefaultHandler { get; protected set; }

		// This is used when the player needs to select an option from the window menu.
		// Which means the player can't press the Esc button and close the window.
		public bool IsAbleToCloseWindow { get; protected set; } = true;
		public UIInterfaceType UiInterfaceType { get => uiInterfaceType; }

		public static event Action<bool> OnPauseEvent;

		protected GameObject lastSelectedGO;

		protected override void OnUpdate()
		{
			if (EventSystem.current.currentSelectedGameObject) return;
			if (!lastSelectedGO) return;

			EventSystem.current.SetSelectedGameObject(lastSelectedGO);
		}

		/// <summary>
		/// Resume time.
		/// </summary>
		public static void ResumeTime()
		{
			OnPauseEvent?.Invoke(false);
		}

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
		public virtual void CloseWindow()
		{
			gameObject.SetActive(false);
			UIManager.Instance.WindowStack.Pop();

			if (UIManager.Instance.WindowStack.Count > 0) return;

			InputManager.Instance.SetToDefaultActionMap();
			OnPauseEvent?.Invoke(false);
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
