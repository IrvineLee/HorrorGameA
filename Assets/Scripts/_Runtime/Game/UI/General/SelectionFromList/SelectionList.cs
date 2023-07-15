using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Helper;

namespace Personal.UI
{
	public class SelectionList : MonoBehaviour
	{
		[SerializeField] Button leftButton = null;
		[SerializeField] Button rightButton = null;
		[SerializeField] TextMeshProUGUI templateTMP = null;

		[Space]
		[SerializeField] float lerpDuration = 0.25f;
		[SerializeField] Transform selectionParentTrans = null;
		[SerializeField] List<string> stringList = new();

		public List<string> StringList { get => stringList; }

		protected int currentActiveIndex;
		protected List<TextMeshProUGUI> stringTMPList = new();

		float lerpWidth;
		bool isInitalized;

		CoroutineRun lerpCR = new();

		void Awake()
		{
			if (stringList.Count <= 0 || isInitalized) return;

			Initialize();
			HandleButtonVisibility();
		}

		public void AddToListAndInitalize(List<string> stringList)
		{
			this.stringList = new List<string>(stringList);
			Initialize();
		}

		public void SetCurrentIndex(int currentActiveIndex)
		{
			if (!isInitalized) Initialize();

			this.currentActiveIndex = currentActiveIndex;
			selectionParentTrans.localPosition = selectionParentTrans.localPosition.With(x: currentActiveIndex * (-lerpWidth));

			HandleButtonVisibility();
		}

		protected virtual void HandleSelectionValueChangedEvent() { }

		protected virtual void Initialize()
		{
			if (stringList.Count <= 0) return;

			// Spawn the required selection to choose from.
			foreach (var str in stringList)
			{
				TextMeshProUGUI tmp = Instantiate(templateTMP, selectionParentTrans);
				Rect rect = templateTMP.rectTransform.rect;

				tmp.rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
				tmp.name = str;
				tmp.text = str;

				stringTMPList.Add(tmp);
			}

			// Get the lerp width and disable the template.
			lerpWidth = templateTMP.rectTransform.rect.width;
			templateTMP.gameObject.SetActive(false);

			// Set the events.
			leftButton.onClick.AddListener(() => HandleSelection(false));
			rightButton.onClick.AddListener(() => HandleSelection(true));

			isInitalized = true;
		}

		void HandleSelection(bool isNext)
		{
			if (!lerpCR.IsDone) return;
			if ((isNext && currentActiveIndex >= stringList.Count - 1) ||
				(!isNext && currentActiveIndex == 0)) return;

			// Scroll throught the list.
			currentActiveIndex = isNext ? currentActiveIndex + 1 : currentActiveIndex - 1;
			float width = isNext ? -lerpWidth : lerpWidth;

			// Lerp to the next selection.
			Vector3 nextPos = selectionParentTrans.localPosition.With(x: selectionParentTrans.localPosition.x + width);
			lerpCR = CoroutineHelper.LerpFromTo(selectionParentTrans, selectionParentTrans.localPosition, nextPos, lerpDuration, default, default, false);

			// Handle the button visibility and selection changed event.
			HandleButtonVisibility();
			HandleSelectionValueChangedEvent();
		}

		void HandleButtonVisibility()
		{
			leftButton.interactable = true;
			rightButton.interactable = true;

			if (currentActiveIndex == 0) leftButton.interactable = false;
			else if (currentActiveIndex >= stringList.Count - 1) rightButton.interactable = false;
		}

		void OnDestroy()
		{
			leftButton.onClick.RemoveAllListeners();
			rightButton.onClick.RemoveAllListeners();
		}
	}
}
