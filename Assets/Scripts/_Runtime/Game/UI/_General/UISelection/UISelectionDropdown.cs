using UnityEngine.Events;

namespace Personal.UI
{
	public class UISelectionDropdown : UISelectionListing
	{
		public int Value { get => currentActiveIndex; }

		public UnityEvent<int> OnValueChanged;

		protected override void HandleSelectionValueChangedEvent()
		{
			OnValueChanged?.Invoke(Value);
		}
	}
}
