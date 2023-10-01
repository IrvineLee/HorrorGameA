using UnityEngine;

namespace Helper
{
	/// <summary>
	/// Follow the content height that might change according to content size fitter.
	/// Use this when the object itself is not part of the parent.
	/// </summary>
	public class FollowContentHeight : MonoBehaviour
	{
		[SerializeField] Transform contentTrans = null;
		[SerializeField] float maxHeight = 128;
		[SerializeField] Vector2 offset = Vector2.zero;

		RectTransform rectTransform;
		RectTransform contentTransRectTransform;

		Canvas canvas;

		void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			contentTransRectTransform = contentTrans.GetComponent<RectTransform>();

			canvas = GetComponentInParent<Canvas>(true);
		}

		void OnEnable()
		{
			HandleLayout();
		}

		void HandleLayout()
		{
			if (!rectTransform || !contentTransRectTransform || !canvas) Awake();

			float scalarHeight = contentTransRectTransform.rect.height * canvas.scaleFactor;
			Vector3 scalarOffset = (Vector3)offset * canvas.scaleFactor;

			float scalarMaxHeight = maxHeight * canvas.scaleFactor;
			if (scalarHeight > scalarMaxHeight) scalarHeight = scalarMaxHeight;

			transform.position = contentTrans.position.With(y: contentTrans.position.y + scalarHeight) + scalarOffset;
		}

		void OnValidate()
		{
			HandleLayout();
		}
	}
}