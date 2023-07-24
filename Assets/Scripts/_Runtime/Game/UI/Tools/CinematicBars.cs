using UnityEngine;
using UnityEngine.UI;
using System;

using Helper;
using Sirenix.OdinInspector;

namespace Personal.UI
{
	public class CinematicBars : MonoBehaviour
	{
		[AssetsOnly]
		[SerializeField] Sprite sprite = null;
		[SerializeField] float duration = 1f;

		[Range(0, 1)]
		[SerializeField] float heightRatio = 0.05f;

		Vector2 referenceResolution;

		RectTransform topBar;
		RectTransform bottomBar;

		CoroutineRun topBarCR = new CoroutineRun();
		CoroutineRun bottomBarCR = new CoroutineRun();

		Vector3 topStartLocalPosition;
		Vector3 bottomStartLocalPosition;

		Vector3 topEndLocalPosition;
		Vector3 bottomEndLocalPosition;

		void Awake()
		{
			CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
			referenceResolution = canvasScaler.referenceResolution;

			float height = heightRatio * referenceResolution.y;

			// Create the bars.
			topBar = CreateBar("TopBar", new Vector2(0, 1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, height));
			bottomBar = CreateBar("BottomBar", new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, height));

			// Get the start local position.
			topStartLocalPosition = topBar.localPosition.With(y: topBar.localPosition.y + height);
			bottomStartLocalPosition = bottomBar.localPosition;

			// Get the end local position.
			topEndLocalPosition = topBar.localPosition;
			bottomEndLocalPosition = bottomBar.localPosition.With(y: bottomBar.localPosition.y + height);

			// Initialize the local position.
			topBar.localPosition = topStartLocalPosition;
			bottomBar.localPosition = bottomStartLocalPosition;
		}

		RectTransform CreateBar(string goName, Vector2 pivot, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta)
		{
			GameObject go = new GameObject(goName, typeof(Image));
			go.transform.SetParent(transform, false);

			Image image = go.GetComponent<Image>();
			image.sprite = sprite;
			image.transform.localScale = image.transform.localScale.With(y: pivot.y == 0 ? -1 : 1);

			RectTransform rectTransform = go.GetComponent<RectTransform>();
			rectTransform.pivot = pivot;
			rectTransform.anchorMin = anchorMin;
			rectTransform.anchorMax = anchorMax;
			rectTransform.sizeDelta = sizeDelta;

			return rectTransform;
		}

		public void Show()
		{
			BeginMovingReverse(topEndLocalPosition, bottomEndLocalPosition);
		}

		public void Hide()
		{
			BeginMovingReverse(topStartLocalPosition, bottomStartLocalPosition);
		}

		void BeginMovingReverse(Vector3 topFinalPosition, Vector3 bottomFinalPosition)
		{
			topBarCR.StopCoroutine();
			bottomBarCR.StopCoroutine();

			topBarCR = CoroutineHelper.LerpFromTo(topBar, topBar.localPosition, topFinalPosition, duration);
			bottomBarCR = CoroutineHelper.LerpFromTo(bottomBar, bottomBar.localPosition, bottomFinalPosition, duration);
		}
	}
}
