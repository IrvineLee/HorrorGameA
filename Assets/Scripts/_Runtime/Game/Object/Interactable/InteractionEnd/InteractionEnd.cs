using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public abstract class InteractionEnd : GameInitialize
	{
		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		public async UniTask EnableInteractables()
		{
			await UniTask.Yield(PlayerLoopTiming.LastTimeUpdate);

			if (!IsEnded()) return;

			HandleInteractable();
			StageManager.Instance.GetReward(rewardInteractableObjectList).Forget();
		}

		protected virtual void HandleInteractable() { }

		protected virtual bool IsEnded() { return true; }
	}
}
