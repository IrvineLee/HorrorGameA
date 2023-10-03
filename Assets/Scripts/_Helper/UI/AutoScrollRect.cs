using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Helper
{
	[RequireComponent(typeof(ScrollRect))]
	public class AutoScrollRect : MonoBehaviour
	{
		[SerializeField] RectTransform viewportRectTransform = null;
		[SerializeField] RectTransform contentRectTransform = null;
		[SerializeField] float duration = 0.1f;

		ScrollRect scrollRect;
		RectTransform selectedRectTransform;

		CoroutineRun moveCR = new();

		void Awake()
		{
			scrollRect = GetComponent<ScrollRect>();
		}

		public void SetSelectionToTop()
		{
			// Wait for the prefab to be instantiated and parented first before setting the ScrollView selection to be the top most.
			CoroutineHelper.WaitNextFrame(() =>
			{
				if (scrollRect.content.childCount <= 0) return;

				scrollRect.verticalNormalizedPosition = 1;
				EventSystem.current.SetSelectedGameObject(scrollRect.content.transform.GetChild(0).gameObject);

			}, isWaitNextEndOfFrame: true);
		}

		public void ScrollToSelected()
		{
			moveCR?.Abort();
			var selected = EventSystem.current.currentSelectedGameObject;

			// nothing is selected, bail
			if (selected == null) return;

			// whatever is selected isn't a descendant of the scroll rect, we can ignore it
			if (!selected.transform.IsChildOf(contentRectTransform)) return;

			selectedRectTransform = selected.GetComponent<RectTransform>();
			HandleMovement();
		}

		void HandleMovement()
		{
			var viewportRect = viewportRectTransform.rect;

			// transform the selected rect from its local space to the content rect space
			var selectedRect = selectedRectTransform.rect;
			var selectedRectWorld = selectedRect.Transform(selectedRectTransform);
			var selectedRectViewport = selectedRectWorld.InverseTransform(viewportRectTransform);

			// now we can calculate if we're outside the viewport either on top or on the bottom
			var outsideOnTop = (selectedRectViewport.yMax - viewportRect.yMax).Round(2);
			var outsideOnBottom = (viewportRect.yMin - selectedRectViewport.yMin).Round(2);

			// if these values are positive, we're outside the viewport
			// if they are negative, we're inside, i zero any "inside" values here to keep things easier to reason about
			if (outsideOnTop < 0) outsideOnTop = 0;
			if (outsideOnBottom < 0) outsideOnBottom = 0;

			// pick the direction to scroll
			// if the selection is big it could possibly be outside on both ends, i prioritize the top here
			var delta = outsideOnTop > 0 ? outsideOnTop : -outsideOnBottom;

			// if no scroll, we bail
			if (delta == 0) return;

			// now we transform the content rect into the viewport space
			var contentRect = contentRectTransform.rect;
			var contentRectWorld = contentRect.Transform(contentRectTransform);
			var contentRectViewport = contentRectWorld.InverseTransform(viewportRectTransform);

			// using this we can calculate how much of the content extends past the viewport
			var overflow = contentRectViewport.height - viewportRect.height;

			// now we can use the overflow from earlier to work out how many units the normalized scroll will move us, so
			// we can scroll exactly to where we need to
			var unitsToNormalized = 1 / overflow;
			var endValue = scrollRect.verticalNormalizedPosition + delta * unitsToNormalized;

			// add in abit of moving animation
			Action<float> callbackMethod = (result) => scrollRect.verticalNormalizedPosition = result;
			Action doLastMethod = () => scrollRect.verticalNormalizedPosition = endValue;

			moveCR = CoroutineHelper.LerpWithinSeconds(scrollRect.verticalNormalizedPosition, endValue, duration, callbackMethod, doLastMethod, isDeltaTime: false);
		}
	}
}