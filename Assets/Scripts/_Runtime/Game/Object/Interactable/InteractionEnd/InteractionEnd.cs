using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.InteractiveObject
{
	public class InteractionEnd : GameInitialize
	{
		[SerializeField] List<InteractableObject> enableInteractableObjectList = new();
		[SerializeField] List<InteractableObject> disableInteractableObjectList = new();

		public async UniTask EnableInteractables()
		{
			await UniTask.Yield(PlayerLoopTiming.LastTimeUpdate);

			if (!IsEnded()) return;
			HandleInteractable();
		}

		protected virtual void HandleInteractable()
		{
			foreach (var interactable in enableInteractableObjectList)
			{
				interactable.SetIsInteractable(true);
			}
			foreach (var interactable in disableInteractableObjectList)
			{
				interactable.SetIsInteractable(false);
			}
		}

		protected virtual bool IsEnded() { return true; }
	}
}
