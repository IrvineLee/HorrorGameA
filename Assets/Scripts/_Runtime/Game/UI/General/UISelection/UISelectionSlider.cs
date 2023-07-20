using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public class UISelectionSlider : UISelectionBase
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
	}
}
