using Personal.Manager;

namespace Personal.UI
{
	public class ButtonInteractOption : ButtonInteractSet
	{
		IWindowHandler iWindowHandler;

		public override void Initialize()
		{
			iWindowHandler = UIManager.Instance.OptionUI.GetComponentInChildren<IWindowHandler>();
			button.onClick.AddListener(OpenWindow);
		}

		void OpenWindow()
		{
			iWindowHandler.OpenWindow();
		}

		void OnApplicationQuit()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
