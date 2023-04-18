using UnityEngine;
using TMPro;

namespace Helper
{
	public class FPSCounter : MonoBehaviour
	{
		[SerializeField] float refresh = 1f;

		string display = "{0} FPS";
		float timer, avgFramerate;
		TextMeshProUGUI tmp;

		void Start()
		{
			tmp = GetComponentInChildren<TextMeshProUGUI>();
		}

		void Update()
		{
			//Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
			float timelapse = Time.smoothDeltaTime;
			timer = timer <= 0 ? refresh : timer -= timelapse;

			if (timer <= 0) avgFramerate = (int)(1f / timelapse);
			tmp.text = string.Format(display, avgFramerate.ToString());
		}
	}
}
