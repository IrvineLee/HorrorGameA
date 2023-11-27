using System;

using Helper;
using Personal.Manager;

namespace Personal.UI
{
	/// <summary>
	/// This is typically for menu window UI.
	/// </summary>
	[Serializable]
	public abstract class MenuUI : MenuUIBase
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
			CoroutineHelper.WaitNextFrame(() =>
			{
				if (!UIManager.IsWindowStackEmpty)
				{
					UIManager.WindowStack.Pop();
					if (!UIManager.IsWindowStackEmpty) return;
				}

				InputManager.Instance.SetToDefaultActionMap();
				OnPause(false);
			});
		}
	}
}
