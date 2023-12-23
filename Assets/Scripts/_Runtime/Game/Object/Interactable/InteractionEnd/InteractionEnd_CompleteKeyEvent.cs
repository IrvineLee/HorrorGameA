using System;
using UnityEngine;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_CompleteKeyEvent : InteractionEnd
	{
		[SerializeField] KeyEventType keyEventType = KeyEventType._200000_None;

		public static event Action<KeyEventType> OnKeyEventCompleted;

		protected override void HandleInteractable()
		{
			base.HandleInteractable();
			OnKeyEventCompleted?.Invoke(keyEventType);
		}
	}
}

