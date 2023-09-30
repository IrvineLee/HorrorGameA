using UnityEngine;

namespace Helper
{
	/// <summary>
	/// This automatically strect rect transform to full size if there is no scrollbar.
	/// Resets to default size when there is scrollbar.
	/// </summary>
	public class StretchRectTransIfNoScrollbar : MonoBehaviour
	{
		[SerializeField] Transform scrollBar = null;

		RectTransform rectTransform;
		Vector2 offsetMin;
		Vector2 offsetMax;

		void Awake()
		{
			rectTransform = GetComponent<RectTransform>();

			offsetMin = rectTransform.offsetMin;
			offsetMax = rectTransform.offsetMax;
		}

		void OnEnable()
		{
			if (!scrollBar)
			{
				rectTransform.offsetMin = offsetMin;
				rectTransform.offsetMax = offsetMax;

				return;
			}

			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
	}
}