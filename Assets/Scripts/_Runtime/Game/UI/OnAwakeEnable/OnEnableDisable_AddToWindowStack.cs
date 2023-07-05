using Personal.Manager;

namespace Personal.UI
{
	public class OnEnableDisable_AddToWindowStack : MenuUIBase
	{
		void OnEnable()
		{
			UIManager.Instance.WindowStack.Push(this);
		}

		void OnDisable()
		{
			if (UIManager.Instance.IsWindowStackEmpty) return;
			UIManager.Instance.WindowStack.Pop();
		}
	}
}
