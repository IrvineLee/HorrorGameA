using Personal.Manager;

namespace Personal.UI
{
	public class OnEnableDisable_AddToWindowStack : MenuUIBase
	{
		// This is not a window, so disable them.
		public override void OpenWindow() { }
		public override void CloseWindow(bool isInstant = false) { }

		void OnEnable()
		{
			UIManager.WindowStack.Push(this);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (UIManager.IsWindowStackEmpty) return;
			UIManager.WindowStack.Pop();
		}
	}
}
