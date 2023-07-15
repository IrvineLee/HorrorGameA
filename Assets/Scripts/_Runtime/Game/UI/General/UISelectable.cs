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

		protected MenuUIBase menuUIBase = null;
		protected WindowSelectionUIAnimator windowSelectionUIAnimator;

		protected List<Selectable> selectableList = new();
		protected List<GameObject> ignoredGOList = new();

		void Awake()
		{
			menuUIBase = GetComponentInParent<MenuUIBase>(true);
			windowSelectionUIAnimator = GetComponentInChildren<WindowSelectionUIAnimator>(true);

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

			eventData.selectedObject = gameObject;
			menuUIBase.SetLastSelectedGO(gameObject);
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			windowSelectionUIAnimator?.Run(true);

			foreach (var selectable in selectableList)
			{
				selectable.targetGraphic.color = selectable.colors.selectedColor;
			}
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

				foreach (var selectable in selectableList)
				{
					selectable.targetGraphic.color = selectable.colors.normalColor;
				}
			});
		}
	}
}
