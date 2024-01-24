using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.InteractiveObject
{
	public abstract class InteractionEnd : GameInitialize
	{
		public async UniTask EnableInteractables()
		{
			await UniTask.Yield(PlayerLoopTiming.LastTimeUpdate);

			if (!IsEnded()) return;

			HandleInteractable();
		}

		protected virtual void HandleInteractable() { }

		protected virtual bool IsEnded() { return true; }
	}
}
