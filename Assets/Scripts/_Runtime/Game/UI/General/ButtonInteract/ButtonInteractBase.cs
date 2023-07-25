using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public abstract class ButtonInteractBase : MonoBehaviour
	{
		protected Button button;
		protected UIGamepadMovement uiGamepadMovement;

		public virtual void InitialSetup()
		{
			button = GetComponentInChildren<Button>();
			uiGamepadMovement = GetComponentInParent<UIGamepadMovement>(true);
		}
	}
}
