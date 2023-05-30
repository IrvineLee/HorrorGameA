using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Personal.Object;
using Helper;

namespace Personal.Character.Player
{
	public class PlayerInventory : MonoBehaviour
	{
		[SerializeField] Transform inventoryView = null;

		[SerializeField]
		[ReadOnly]
		InteractableObject activeObject = null;

		[SerializeField]
		[ReadOnly]
		List<InteractableObject> interactableObjectList = new();

		public InteractableObject ActiveObject { get => activeObject; }
		public List<InteractableObject> InteractableObjectList { get => interactableObjectList; }

		int currentActiveIndex;

		/// <summary>
		/// Add item to inventory.
		/// </summary>
		/// <param name="interactableObject"></param>
		public void AddItem(InteractableObject interactableObject)
		{
			activeObject = interactableObject;

			SetToInventoryView();
			interactableObjectList.Add(interactableObject);

			currentActiveIndex = interactableObjectList.Count - 1;
		}

		/// <summary>
		/// Switch between items.
		/// </summary>
		/// <param name="isUp"></param>
		public void MouseScrollView(bool isUp)
		{
			currentActiveIndex = isUp ? currentActiveIndex + 1 : currentActiveIndex - 1;
			currentActiveIndex = currentActiveIndex.WithinCount(interactableObjectList.Count);
		}

		/// <summary>
		/// Put it near the player view.
		/// </summary>
		void SetToInventoryView()
		{
			Transform activeTrans = activeObject.transform;
			activeTrans.SetParent(inventoryView);
			activeTrans.localPosition = Vector3.zero;
			activeTrans.localRotation = Quaternion.identity;
			activeTrans.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		}
	}
}