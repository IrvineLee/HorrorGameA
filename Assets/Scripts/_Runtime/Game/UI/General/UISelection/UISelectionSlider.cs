using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public class UISelectionSlider : UISelectionBase
	{
		Slider slider;

		protected override void Initialize()
		{
			slider = GetComponentInChildren<Slider>();
		}

		public override void NextSelection(bool isNext)
		{
			slider.value = isNext ? slider.value + 1 : slider.value - 1;
		}
	}
}
