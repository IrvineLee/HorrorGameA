using System;
using UnityEngine;
using UnityEngine.UI;

using Helper;

namespace Personal.UI
{
	public class PaddingAnimation : MonoBehaviour
	{
		[SerializeField] RectOffset startRectOffset = null;
		[SerializeField] float duration = 0.25f;

		RectTransform rectTrans;
		RectOffset rectOffset;

		void Awake()
		{
			rectTrans = GetComponentInChildren<RectTransform>();
			rectOffset = GetComponentInChildren<LayoutGroup>()?.padding;
		}

		void OnEnable()
		{
			if (rectOffset == null) return;

			Action<int> callbackMethodLeft = (result) => rectOffset.left = result;
			CoroutineHelper.LerpWithinSeconds(startRectOffset.left, rectOffset.left, duration, callbackMethodLeft);

			Action<int> callbackMethodRight = (result) => rectOffset.right = result;
			CoroutineHelper.LerpWithinSeconds(startRectOffset.right, rectOffset.right, duration, callbackMethodRight);

			Action<int> callbackMethodTop = (result) => rectOffset.top = result;
			CoroutineHelper.LerpWithinSeconds(startRectOffset.top, rectOffset.top, duration, callbackMethodTop);

			Action<int> callbackMethodBottom = (result) => { rectOffset.bottom = result; LayoutRebuilder.MarkLayoutForRebuild(rectTrans); };
			CoroutineHelper.LerpWithinSeconds(startRectOffset.bottom, rectOffset.bottom, duration, callbackMethodBottom);
		}
	}
}