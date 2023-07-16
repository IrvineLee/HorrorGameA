using UnityEngine.Events;

namespace Personal.UI
{
	public class DropdownSelectionList : SelectionListing
	{
		public int Value { get => currentActiveIndex; }

		public UnityEvent<int> OnValueChanged;

		protected override void HandleSelectionValueChangedEvent()
		{
			OnValueChanged?.Invoke(Value);
		}
	}
}
