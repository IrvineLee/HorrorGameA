using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Helper;

namespace Personal.UI
{
	public class UISelectionListing : UISelectionBase
	{
		[SerializeField] protected Button leftButton = null;
		[SerializeField] protected Button rightButton = null;
		[SerializeField] protected TextMeshProUGUI templateTMP = null;

		[Space]
		[SerializeField] protected float lerpDuration = 0.25f;
		[SerializeField] protected Transform selectionParentTrans = null;

		[Tooltip("Only use this if you have pre-determined string to display to user.")]
		[SerializeField] protected List<string> stringList = new();

		public List<string> StringList { get => stringList; }

		protected int currentActiveIndex;

		// All the spawned TMP.
		protected List<TextMeshProUGUI> stringTMPList = new();

		// Currently active TMP.
		protected List<TextMeshProUGUI> activeTMPList = new();

		float lerpWidth;
		float spacing;

		CoroutineRun lerpCR = new();

		protected virtual void HandleSelectionValueChangedEvent() { }

		public override void InitialSetup()
		{
			var layoutGroup = selectionParentTrans.GetComponentInChildren<HorizontalLayoutGroup>();
			if (layoutGroup) spacing = layoutGroup.spacing;
		}

		/// <summary>
		/// Initialize the pre-defined values in inspector.
		/// </summary>
		public override void Initialize()
		{
			if (stringList.Count <= 0) return;

			SpawnRequiredSelection(stringList);
			HandleButtonVisibility();
		}

		/// <summary>
		/// This is used to add the selections to the list after starting the game.
		/// Use this to update the stringList(pre-determined string).
		/// Useful for screen resolutions etc.
		/// </summary>
		/// <param name="stringList"></param>
		public void UpdateListAndInitalize(List<string> stringList)
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
			if (stringTMPList.Count <= 0) return;
			if (!lerpCR.IsDone) return;
			if ((isNext && currentActiveIndex >= activeTMPList.Count - 1) ||
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

		protected void SpawnRequiredSelection(List<string> strList)
		{
			SpawnSelectionAddData(strList);

			// Get the lerp width and disable the template.
			lerpWidth = templateTMP.rectTransform.rect.width + spacing;
			templateTMP.gameObject.SetActive(false);

			// Set the events.
			leftButton.onClick.AddListener(() => NextSelection(false));
			rightButton.onClick.AddListener(() => NextSelection(true));
		}

		protected void SpawnSelectionAddData(List<string> strList)
		{
			// Deactivate all tmp.
			stringTMPList.DisableAllGameObject();
			activeTMPList.Clear();

			Rect rect = templateTMP.rectTransform.rect;
			for (int i = 0; i < strList.Count; i++)
			{
				if (stringTMPList.Count <= i)
				{
					// Spawn the required selection to choose from.
					TextMeshProUGUI tmp = Instantiate(templateTMP, selectionParentTrans);
					stringTMPList.Add(tmp);
				}

				TextMeshProUGUI currentTMP = stringTMPList[i];
				string currentStr = strList[i];

				currentTMP.rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
				currentTMP.name = currentStr;
				currentTMP.text = currentStr;
				currentTMP.gameObject.SetActive(true);

				activeTMPList.Add(currentTMP);
			}
		}

		protected virtual void HandleButtonVisibility()
		{
			leftButton.interactable = true;
			rightButton.interactable = true;

			if (currentActiveIndex == 0) leftButton.interactable = false;
			else if (currentActiveIndex >= activeTMPList.Count - 1) rightButton.interactable = false;
		}

		void OnDestroy()
		{
			leftButton.onClick.RemoveAllListeners();
			rightButton.onClick.RemoveAllListeners();
		}
	}
}
