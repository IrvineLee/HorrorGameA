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

			MoveLeft(startRectOffset.left, rectOffset.left);
			MoveRight(startRectOffset.right, rectOffset.right);
			MoveTop(startRectOffset.top, rectOffset.top);
			MoveBottom(startRectOffset.bottom, rectOffset.bottom);
		}

		public void MoveOut()
		{
			MoveLeft(rectOffset.left, startRectOffset.left);
			MoveRight(rectOffset.right, startRectOffset.right);
			MoveTop(rectOffset.top, startRectOffset.top);
			MoveBottom(rectOffset.bottom, startRectOffset.bottom);
		}

		void MoveLeft(int startValue, int endValue)
		{
			Action<int> callbackMethodLeft = (result) => rectOffset.left = result;
			CoroutineHelper.LerpWithinSeconds(startValue, endValue, duration, callbackMethodLeft);
		}

		void MoveRight(int startValue, int endValue)
		{
			Action<int> callbackMethodRight = (result) => rectOffset.right = result;
			CoroutineHelper.LerpWithinSeconds(startValue, endValue, duration, callbackMethodRight);
		}

		void MoveTop(int startValue, int endValue)
		{
			Action<int> callbackMethodTop = (result) => rectOffset.top = result;
			CoroutineHelper.LerpWithinSeconds(startValue, endValue, duration, callbackMethodTop);
		}

		void MoveBottom(int startValue, int endValue)
		{
			Action<int> callbackMethodBottom = (result) => { rectOffset.bottom = result; LayoutRebuilder.MarkLayoutForRebuild(rectTrans); };
			CoroutineHelper.LerpWithinSeconds(startValue, endValue, duration, callbackMethodBottom);
		}
	}
}