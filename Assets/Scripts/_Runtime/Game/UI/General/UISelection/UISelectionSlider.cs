using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Helper;

namespace Personal.UI
{
	public class UISelectionSlider : UISelectionBase, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField] float addValue = 1;
		[SerializeField] float holdDownAddMultiplier = 1;
		[SerializeField] float holdDownMaxMultiplier = 3;
		[SerializeField] float addMultiplierAfterDuration = 2f;

		Slider slider;
		float multiplier = 1f;

		CoroutineRun holdCR = new();
		bool isIncreaseMultiplier;

		public override void Initialize()
		{
			slider = GetComponentInChildren<Slider>();
		}

		public override void NextSelection(bool isNext)
		{
			HandleMultiplier();
			slider.value = isNext ? slider.value + addValue * multiplier : slider.value - addValue * multiplier;
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			UISelectable.LockSelection(true);
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			UISelectable.LockSelection(false);
		}

		void HandleMultiplier()
		{
			if (!UIKeyboardAndGamepadMovement.IsHold)
			{
				holdCR?.StopCoroutine();
				isIncreaseMultiplier = false;

				multiplier = 1f;
				return;
			}

			if (isIncreaseMultiplier)
			{
				multiplier += holdDownAddMultiplier;
				if (multiplier > holdDownMaxMultiplier)
				{
					multiplier = holdDownMaxMultiplier;
				}
			}
			else if (holdCR.IsDone)
			{
				holdCR = CoroutineHelper.WaitFor(addMultiplierAfterDuration, () => isIncreaseMultiplier = true, default, true);
			}
		}
	}
}
