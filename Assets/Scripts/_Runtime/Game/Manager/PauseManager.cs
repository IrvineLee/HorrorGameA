using UnityEngine;
using UnityEngine.EventSystems;

using Helper;
using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.UI;

namespace Personal.Manager
{
	public class PauseManager : GameInitializeSingleton<PauseManager>
	{
		public bool IsPaused { get; private set; }

		/// <summary>
		/// Resume time.
		/// </summary>
		public void ResumeTime()
		{
			Pause(false);
		}

		protected override void Initialize()
		{
			MenuUI.OnPauseEvent += Pause;
		}

		void Pause(bool isFlag)
		{
			if (IsPaused == isFlag) return;
			if (!GameSceneManager.Instance.IsMainScene()) return;

			IsPaused = isFlag;
			Time.timeScale = isFlag ? 0 : 1;

			// You don't wanna affect the cursor during dialogue/when not using mouse.
			if (UIManager.Instance.ActiveInterfaceType == UIInterfaceType.Dialogue) return;
			if (UIManager.Instance.ActiveInterfaceType == UIInterfaceType.Debug) return;

			// When pausing the game, make sure the appearing window animation is complete before setting the mouse cursor.
			if (isFlag)
			{
				HandleWindowAnimation();
				return;
			}

			CursorManager.Instance.HandleMouse();
		}

		void HandleWindowAnimation()
		{
			CoroutineHelper.WaitNextFrame(async () =>
			{
				var currentGO = EventSystem.current.currentSelectedGameObject;
				if (!currentGO) return;

				// Set the cursor to the selected response or center of the screen.
				var uiSelectable = currentGO.GetComponentInChildren<UISelectable>();
				if (uiSelectable)
				{
					//Debug.Log("Inside " + uiSelectable.name + "   " + uiSelectable.transform.parent);
					await UniTask.WaitUntil(() => uiSelectable.WindowSelectionUIAnimator.IsDone);
				}

				CursorManager.Instance.HandleMouse();
			}, isEndOfFrame: true);

		}

		void OnApplicationQuit()
		{
			MenuUI.OnPauseEvent -= Pause;
		}
	}
}