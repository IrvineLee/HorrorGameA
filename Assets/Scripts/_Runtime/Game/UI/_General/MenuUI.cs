using System;

using Helper;
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

		public override void CloseWindow(bool isInstant = false)
		{
			if (!IsWindowAnimationDone && !isInstant) return;

			EnableGO(false, isInstant);

			if (isInstant)
			{
				RevertToDefaultState();
				return;
			}
			CoroutineHelper.WaitNextFrame(RevertToDefaultState);
		}

		void RevertToDefaultState()
		{
			UIManager.WindowStack.Pop();
			if (!UIManager.IsWindowStackEmpty) return;

			InputManager.Instance.SetToDefaultActionMap();
			OnPause(false);
		}
	}
}
