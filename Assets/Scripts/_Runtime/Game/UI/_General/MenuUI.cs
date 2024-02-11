using System;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.UI
{
	/// <summary>
	/// This is typically for menu window UI.
	/// </summary>
	[Serializable]
	public class MenuUI : MenuUIBase
	{
		public override void OpenWindow()
		{
			if (!IsWindowAnimationDone) return;

			UIManager.WindowStack.Push(this);
			EnableGO(true, false);

			OnPause(true);
		}

		public override async UniTask CloseWindow(bool isInstant = false)
		{
			if (!IsWindowAnimationDone && !isInstant) return;

			EnableGO(false, isInstant);
			if (isInstant)
			{
				RevertToDefaultState();
				return;
			}

			await UniTask.NextFrame();

			// Pop the window stack here so other script can handle their own assessment.
			UIManager.WindowStack.Pop();
			CursorManager.Instance.HideMouseCursor();

			await UniTask.WaitUntil(() => IsWindowAnimationDone);
			RevertToDefaultState();
		}

		void RevertToDefaultState()
		{
			windowUIAnimator.gameObject.SetActive(false);

			if (!UIManager.IsWindowStackEmpty) return;

			InputManager.Instance.SetToDefaultActionMap();
			OnPause(false);
		}
	}
}
