using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public class ScrollbarReset : MonoBehaviour
	{
		Scrollbar scrollbar;

		float scrollbarValue;
		float scrollbarSize;

		void Awake()
		{
			scrollbar = GetComponentInChildren<Scrollbar>();
			scrollbarValue = scrollbar.value;
			scrollbarSize = scrollbar.size;
		}

		void OnEnable()
		{
			scrollbar.value = scrollbarValue;
			scrollbar.size = scrollbarSize;
		}
	}
}
