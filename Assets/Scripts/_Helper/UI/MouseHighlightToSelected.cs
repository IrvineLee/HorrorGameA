using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Helper
{
	[RequireComponent(typeof(Selectable))]
	public class MouseHighlightToSelected : MonoBehaviour, IPointerEnterHandler
	{
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (!EventSystem.current.alreadySelecting)
				EventSystem.current.SetSelectedGameObject(gameObject);
		}
	}
}