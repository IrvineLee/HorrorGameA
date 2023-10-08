using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public abstract class ButtonInteractBase : MonoBehaviour
	{
		protected Button button;

		public virtual void InitialSetup()
		{
			button = GetComponentInChildren<Button>();
		}
	}
}
