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

		CoroutineRun waitCR = new();
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
			if (UIGamepadMovement.IsHold)
			{
				if (!isIncreaseMultiplier && waitCR.IsDone)
				{
					waitCR = CoroutineHelper.WaitFor(addMultiplierAfterDuration, () => isIncreaseMultiplier = true);
				}
			}
			else
			{
				waitCR?.StopCoroutine();
				isIncreaseMultiplier = false;
			}

			if (!isIncreaseMultiplier)
			{
				multiplier = 1f;
				return;
			}

			// Increase multiplier.
			multiplier += holdDownAddMultiplier;
			if (multiplier > holdDownMaxMultiplier)
			{
				multiplier = holdDownMaxMultiplier;
			}
		}
	}
}
