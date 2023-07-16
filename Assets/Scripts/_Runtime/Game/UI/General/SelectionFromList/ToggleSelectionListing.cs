using UnityEngine.Events;

namespace Personal.UI
{
	public class ToggleSelectionListing : SelectionListing
	{
		public bool IsOn { get => currentActiveIndex != 0; }

		public UnityEvent<bool> OnValueChanged;

		protected override void HandleSelectionValueChangedEvent()
		{
			OnValueChanged?.Invoke(IsOn);
		}
	}
}
