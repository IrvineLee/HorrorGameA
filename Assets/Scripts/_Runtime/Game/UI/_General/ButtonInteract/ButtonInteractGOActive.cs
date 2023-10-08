using UnityEngine;

namespace Personal.UI
{
	public class ButtonInteractGOActive : ButtonInteractBase
	{
		[SerializeField] GameObject activeGO = null;
		[SerializeField] bool isActive = true;

		public override void InitialSetup()
		{
			base.InitialSetup();
			button.onClick.AddListener(ActivateGO);
		}

		void ActivateGO()
		{
			activeGO.SetActive(isActive);
		}

		void OnApplicationQuit()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
