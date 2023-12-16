using System.Collections.Generic;
using UnityEngine;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_GameObject : InteractionEnd
	{
		[SerializeField] List<GameObject> enableGOList = new();
		[SerializeField] List<GameObject> disableGOList = new();

		protected override void HandleInteractable()
		{
			foreach (var go in enableGOList)
			{
				go.SetActive(true);
			}
			foreach (var go in disableGOList)
			{
				go.SetActive(false);
			}
		}
	}
}
