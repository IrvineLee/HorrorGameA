using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Helper;

namespace Personal.UI
{
	public class UISelectionListing : UISelectionBase
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

		CoroutineRun lerpCR = new();

		protected virtual void HandleSelectionValueChangedEvent() { }

		/// <summary>
		/// Initialize the pre-defined values in inspector.
		/// </summary>
		public override void Initialize()
		{
			if (stringList.Count <= 0) return;

			SpawnRequiredSelection();
			HandleButtonVisibility();
		}

		/// <summary>
		/// This is used to add the selections to the list after starting the game.
		/// Useful for screen resolutions etc.
		/// </summary>
		/// <param name="stringList"></param>
		public void AddToListAndInitalize(List<string> stringList)
		{
			this.stringList = new List<string>(stringList);
			Initialize();
		}

		/// <summary>
		/// Used to set the current index in the list.
		/// </summary>
		/// <param name="currentActiveIndex"></param>
		public void SetCurrentIndex(int currentActiveIndex)
		{
			this.currentActiveIndex = currentActiveIndex;
			selectionParentTrans.localPosition = selectionParentTrans.localPosition.With(x: currentActiveIndex * (-lerpWidth));

			HandleButtonVisibility();
		}

		/// <summary>
		/// Button pressed left or right.
		/// </summary>
		/// <param name="isNext"></param>
		public override void NextSelection(bool isNext)
		{
			if (stringList.Count <= 0) return;
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

		void SpawnRequiredSelection()
		{
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
			leftButton.onClick.AddListener(() => NextSelection(false));
			rightButton.onClick.AddListener(() => NextSelection(true));
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
