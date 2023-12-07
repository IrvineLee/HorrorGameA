using UnityEngine;
using UnityEngine.UI;

namespace Personal.Network
{
	public class URLConnect : MonoBehaviour
	{
		[SerializeField] string url = "";
		[SerializeField] bool autoButtonEvent = true;

		Button button;

		void Awake()
		{
			if (!autoButtonEvent) return;

			button = GetComponentInChildren<Button>();
			button?.onClick.AddListener(Connect);
		}

		/// <summary>
		/// Inspector : Attach this on the button event.
		/// </summary>
		public void Connect()
		{
			Application.OpenURL(url);
		}

		void OnDestroy()
		{
			button?.onClick.RemoveAllListeners();
		}
	}
}