using Personal.Manager;

namespace Personal.UI
{
	public class OnEnableDisable_AddToWindowStack : MenuUIBase
	{
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
