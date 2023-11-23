using Personal.Manager;

namespace Personal.UI
{
	public class OnEnableDisable_AddToWindowStack : MenuUIBase
	{
		void OnEnable()
		{
			UIManager.WindowStack.Push(this);
		}

		void OnDisable()
		{
			if (UIManager.IsWindowStackEmpty) return;
			UIManager.WindowStack.Pop();
		}
	}
}
