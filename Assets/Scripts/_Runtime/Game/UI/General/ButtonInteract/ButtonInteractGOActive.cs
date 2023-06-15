using UnityEngine;

namespace Personal.UI
{
	public class ButtonInteractGOActive : ButtonInteractSet
	{
		[SerializeField] GameObject activeGO = null;
		[SerializeField] bool isActive = true;

		public override void Initialize()
		{
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
