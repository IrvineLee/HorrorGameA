using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public class ButtonInteractSet : MonoBehaviour
	{
		protected Button button;

		void Awake()
		{
			button = GetComponentInChildren<Button>();
		}

		public virtual void Initialize() { }
	}
}
