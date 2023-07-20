using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Personal.Manager;
using Personal.UI.Window;
using Helper;

namespace Personal.UI
{
	public class UISelectable : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
	{
		[SerializeField] bool isInitialSelection = false;

		public UISelectionBase UISelectionBase { get; private set; }

		protected UIGamepadMovement uiGamepadMovement;

		protected MenuUIBase menuUIBase = null;
		protected WindowSelectionUIAnimator windowSelectionUIAnimator;

		protected List<Selectable> selectableList = new();
		protected List<GameObject> ignoredGOList = new();

		void Awake()
		{
			uiGamepadMovement = GetComponentInParent<UIGamepadMovement>(true);
			menuUIBase = GetComponentInParent<MenuUIBase>(true);
			windowSelectionUIAnimator = GetComponentInChildren<WindowSelectionUIAnimator>(true);

			UISelectionBase = GetComponentInChildren<UISelectionBase>(true);
			selectableList = GetComponentsInChildren<Selectable>(true).ToList();

			// Since only sliders do not follow the usual ui selection, get the sliders and add it here.
			ignoredGOList.AddRange(GetComponentsInChildren<Slider>(true).ToList().Select((slider) => slider.gameObject));
			ignoredGOList.Add(gameObject);
		}

		void OnEnable()
		{
			if (!isInitialSelection) return;

			EventSystem.current.SetSelectedGameObject(gameObject);
			menuUIBase.SetLastSelectedGO(gameObject);
		}

		public void AddIgnoredSelection(List<GameObject> goList)
		{
			ignoredGOList.AddRange(goList);
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (UIManager.Instance.ActiveInterfaceType != menuUIBase.UiInterfaceType) return;
			if (!InputManager.Instance.IsCurrentDeviceMouse) return;

			eventData.selectedObject = gameObject;
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			windowSelectionUIAnimator?.Run(true);
			SetSelectableColor(true);

			menuUIBase.SetLastSelectedGO(gameObject);
			uiGamepadMovement?.SetCurrentIndex(transform.GetSiblingIndex());
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			// Wait for end of frame to check whether the next active selection is within the ignored list.
			CoroutineHelper.WaitEndOfFrame(() =>
			{
				foreach (var ignoredGO in ignoredGOList)
				{
					if (EventSystem.current.currentSelectedGameObject == ignoredGO)
					{
						EventSystem.current.SetSelectedGameObject(gameObject);
						return;
					}
				}

				windowSelectionUIAnimator?.Run(false);
				SetSelectableColor(false);
			});
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
			windowSelectionUIAnimator?.StopAnimation();
			SetSelectableColor(false);
		}
	}
}
