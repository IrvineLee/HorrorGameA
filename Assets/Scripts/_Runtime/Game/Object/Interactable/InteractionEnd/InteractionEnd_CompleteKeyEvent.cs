using UnityEngine;

using Personal.Manager;
using Personal.KeyEvent;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_CompleteKeyEvent : InteractionEnd
	{
		[SerializeField] KeyEventType keyEventType = KeyEventType.None;

		protected override void HandleInteractable()
		{
			base.HandleInteractable();
			StageManager.Instance.RegisterKeyEvent(keyEventType);
		}
	}
}