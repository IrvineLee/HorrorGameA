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

		Slider slider;
		StringBuilder stringBuilder = new StringBuilder();

		void Start()
		{
			slider = GetComponent<Slider>();

			UnityAction<float> unityAction = default;
			unityAction += UpdateText;
			slider.onValueChanged.AddListener(unityAction);

			UpdateText(slider.value);
		}

		void UpdateText(float value)
		{
			stringBuilder.Length = 0;
			stringBuilder.Append(value);
			valueTMP.text = stringBuilder.ToString();
		}
	}

}