using UnityEngine;
using UnityEngine.EventSystems;

using Helper;

namespace Personal.UI
{
	public class UISelectableSelectionList : UISelectable, IDeselectHandler
	{
		[SerializeField] SelectionList selectionList = null;

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			CoroutineHelper.WaitEndOfFrame(() =>
			{
				foreach (var sameSelectable in selectionList.SameUISelectableList)
				{
					if (EventSystem.current.currentSelectedGameObject != sameSelectable) continue;

					menuUIBase.SetLastSelectedGO(gameObject);
					return;
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
