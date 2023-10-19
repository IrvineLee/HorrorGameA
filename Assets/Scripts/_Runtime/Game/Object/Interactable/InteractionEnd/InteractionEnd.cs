using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.InteractiveObject
{
	public class InteractionEnd : GameInitialize
	{
		[SerializeField] List<InteractableObject> interactableObjectList = new();
		[SerializeField] bool isSetTrue = true;

		public async UniTask EnableInteractables()
		{
			await UniTask.Yield(PlayerLoopTiming.LastTimeUpdate);

			if (!IsEnded()) return;
			if (interactableObjectList.Count <= 0) return;

			foreach (var interactable in interactableObjectList)
			{
				interactable.SetIsInteractable(isSetTrue);
			}
		}

		protected virtual bool IsEnded() { return true; }
	}
}

