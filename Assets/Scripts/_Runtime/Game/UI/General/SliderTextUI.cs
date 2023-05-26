using UnityEngine;
using UnityEngine.UI;

using TMPro;
using UnityEngine.Events;
using System.Text;

namespace Personal.UI
{
	[RequireComponent(typeof(Slider))]
	public class SliderTextUI : MonoBehaviour
	{
		[Tooltip("This is used to update the value for slider")]
		[SerializeField] TextMeshProUGUI valueTMP = null;
		[SerializeField] int decimalPoint = 0;

		Slider slider;
		StringBuilder stringBuilder = new StringBuilder();

		string decimalStr;

		void Start()
		{
			slider = GetComponent<Slider>();

			slider.onValueChanged.AddListener(UpdateText);
			UpdateText(slider.value);
		}

		void UpdateText(float value)
		{
			decimalStr = decimalPoint == 0 ? string.Empty : "F" + decimalPoint;

			stringBuilder.Length = 0;
			stringBuilder.Append(value.ToString(decimalStr));
			valueTMP.text = stringBuilder.ToString();
		}
	}
}