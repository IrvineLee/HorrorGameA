using System.Collections.Generic;
using UnityEngine;

using Personal.Object;
using Sirenix.OdinInspector;

namespace Personal.Character.Player
{
	public class PlayerInventory : MonoBehaviour
	{
		[SerializeField]
		[ReadOnly]
		List<InteractableObject> interactableObjectList = new();

		public List<InteractableObject> InteractableObjectList { get => interactableObjectList; }

		public void AddItem(InteractableObject interactableObject)
		{
			interactableObjectList.Add(interactableObject);
		}
	}
}