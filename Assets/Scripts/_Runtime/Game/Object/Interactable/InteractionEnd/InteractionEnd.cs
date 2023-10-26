using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractionEnd : GameInitialize
	{
		[SerializeField] List<InteractableObject> enableInteractableObjectList = new();
		[SerializeField] List<InteractableObject> disableInteractableObjectList = new();
		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		public async UniTask EnableInteractables()
		{
			await UniTask.Yield(PlayerLoopTiming.LastTimeUpdate);

			if (!IsEnded()) return;

			HandleInteractable();
			StageManager.Instance.GetReward(rewardInteractableObjectList).Forget();
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
