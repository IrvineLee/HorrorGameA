using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.FSM;

namespace Personal.InteractiveObject
{
	public abstract class InteractionEnd : GameInitialize
	{
		StateBase stateBase;

		protected override void OnEnabled()
		{
			stateBase = GetComponentInChildren<StateBase>();
			stateBase.OnExitEvent += EnableInteractable;
		}

		void EnableInteractable()
		{
			BeginInteractable().Forget();
		}

		async UniTask BeginInteractable()
		{
			await UniTask.Yield();
			if (!IsEnded()) return;

			HandleInteractable();
		}

		protected virtual void HandleInteractable() { }

		protected virtual bool IsEnded() { return true; }

		protected override void OnDisabled()
		{
			stateBase.OnExitEvent -= EnableInteractable;
		}
	}
}
