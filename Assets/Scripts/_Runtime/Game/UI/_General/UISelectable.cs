using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Helper;
using Personal.Manager;
using Personal.UI.Window;
using Personal.InputProcessing;

namespace Personal.UI
{
	/// <summary>
	/// This is put on selectable objects like buttons, toggle, slider etc under ScrollView content.
	/// Only handles the selection display UI.
	/// </summary>
	public class UISelectable : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
	{
		[SerializeField] bool isInitialSelection = false;

		[Tooltip("This window selection gameobject will get activated/deactivated.")]
		[SerializeField] WindowSelectionUIAnimator windowSelectionUIAnimator = null;

		[Tooltip("This selection will NOT get activated/deactivated.")]
		[SerializeField] WindowSelectionUIAnimator activeSelectionUIAnimator = null;

		public UISelectionBase UISelectionBase { get; private set; }
		public WindowSelectionUIAnimator WindowSelectionUIAnimator { get => windowSelectionUIAnimator; }

		protected ControlInput controlInputUI;

		protected MenuUIBase menuUIBase = null;

		protected List<Selectable> selectableList = new();
		protected List<GameObject> ignoredGOList = new();

		static bool isLockSelection;

		bool isAppearSelected;
		static List<UISelectable> appearSelectedList = new();

		void Awake()
		{
			controlInputUI = GetComponentInParent<ControlInput>(true);
			menuUIBase = GetComponentInParent<MenuUIBase>(true);

			UISelectionBase = GetComponentInChildren<UISelectionBase>(true);
			selectableList = GetComponentsInChildren<Selectable>(true).ToList();

			// Since only sliders do not follow the usual ui selection, get the sliders and add it here.
			ignoredGOList.AddRange(GetComponentsInChildren<Slider>(true).ToList().Select((slider) => slider.gameObject));
			ignoredGOList.Add(gameObject);
		}

		void OnEnable()
		{
			if (!isInitialSelection) return;

			// If you are not the first active sibling, do not select it.
			if (!transform.IsFirstActiveSibling()) return;

			// Make sure it's always on the selected state when starting.
			EventSystem.current.SetSelectedGameObject(gameObject);
			menuUIBase?.SetLastSelectedGO(gameObject);
		}

		/// <summary>
		/// Call this to prevent the selection changing when moving the mouse.
		/// </summary>
		/// <param name="isFlag"></param>
		public static void LockSelection(bool isFlag) { isLockSelection = isFlag; }

		/// <summary>
		/// Since technically there can only be 1 selected gameobject, call this to display it as selected (UI).
		/// The real selection can be somewhere else.
		/// </summary>
		public static void CurrentAppearSelected()
		{
			var uiSelectable = EventSystem.current.currentSelectedGameObject?.GetComponentInChildren<UISelectable>();
			if (!uiSelectable) return;

			uiSelectable.isAppearSelected = true;
			appearSelectedList.Add(uiSelectable);
		}

		public void AddIgnoredSelection(List<GameObject> goList)
		{
			ignoredGOList.AddRange(goList);
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (isLockSelection) return;
			if (UIManager.Instance.ActiveInterfaceType != menuUIBase.UiInterfaceType) return;
			if (!InputManager.Instance.IsCurrentDeviceMouse) return;

			eventData.selectedObject = gameObject;

			// Remove the last appeared selected when mouse-over, so it does not remain on when mouse-overing other selectables.
			if (appearSelectedList.Count > 0)
			{
				int lastIndex = appearSelectedList.Count - 1;
				var lastAppearSelected = appearSelectedList[lastIndex];

				// Only remove when it's in the same category.
				if (UIManager.Instance.ActiveInterfaceType != lastAppearSelected.menuUIBase.UiInterfaceType) return;

				lastAppearSelected.isAppearSelected = false;
				lastAppearSelected.Deselect();

				appearSelectedList.RemoveAt(lastIndex);
			}
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (isLockSelection) return;

			isAppearSelected = false;
			SetSelectableColor(true);

			windowSelectionUIAnimator?.Run(true);
			activeSelectionUIAnimator?.Run(true);

			menuUIBase?.SetLastSelectedGO(gameObject);
			controlInputUI?.UpdateCurrentSelection(gameObject);
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			if (App.IsQuitting) return;

			// You most probably don't wanna OnDeselect when it's busy.
			if (isLockSelection || StageManager.Instance.IsBusy) return;

			// Wait for end of frame to check whether the next active selection is within the ignored list.
			CoroutineHelper.WaitEndOfFrame(Deselect);
		}

		void Deselect()
		{
			if (isAppearSelected) return;

			foreach (var ignoredGO in ignoredGOList)
			{
				if (EventSystem.current.currentSelectedGameObject == ignoredGO)
				{
					EventSystem.current.SetSelectedGameObject(gameObject);
					return;
				}
			}

			SetSelectableColor(false);

			windowSelectionUIAnimator?.Run(false);
			activeSelectionUIAnimator?.Run(false);
		}

		void SetSelectableColor(bool isSelectedColor)
		{
			foreach (var selectable in selectableList)
			{
				selectable.targetGraphic.color = isSelectedColor ? selectable.colors.selectedColor : selectable.colors.normalColor;
			}
		}

		void OnDisable()
		{
			EventSystem.current?.SetSelectedGameObject(null);

			SetSelectableColor(false);

			windowSelectionUIAnimator?.StopAndResetAnimation();
			activeSelectionUIAnimator?.StopAndResetAnimation(false);
		}
	}
}
