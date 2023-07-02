using Personal.Manager;

namespace Personal.UI
{
	public class OnEnableDisable_AddToWindowStack : MenuUIBase
	{
		protected override void OnEnable()
		{
			UIManager.Instance.WindowStack.Push(this);
		}

		protected override void OnDisable()
		{
			if (UIManager.Instance.IsWindowStackEmpty) return;
			UIManager.Instance.WindowStack.Pop();
		}
	}
}
