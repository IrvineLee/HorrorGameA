using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Personal.UI
{
	public class UISelectionSlider : UISelectionBase, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField] float addValue = 1;
		Slider slider;

		public override void Initialize()
		{
			slider = GetComponentInChildren<Slider>();
		}

		public override void NextSelection(bool isNext)
		{
			slider.value = isNext ? slider.value + addValue : slider.value - addValue;
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			UISelectable.LockSelection(true);
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			UISelectable.LockSelection(false);
		}
	}
}
