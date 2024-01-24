using UnityEngine;

using Personal.Manager;

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