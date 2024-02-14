using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.GameState;
using Personal.Manager;

namespace Personal.FSM
{
	public abstract class StateMachineBase : GameInitialize
	{
		[ShowInInspector]
		public StateBase CurrentState { get => state; }
		public StateMachineBase InitiatorStateMachine { get; protected set; }
		public bool IsPauseStateMachine { get => PauseManager.Instance.IsPaused; }

		protected StateBase state;

		protected async UniTask SetState(StateBase stateBase)
		{
			if (state != null && !state.IsWaitForOnExit)
			{
				await UniTask.WaitUntil(() => !IsPauseStateMachine);
				state.OnExit().Forget();
			}

			await UniTask.WaitUntil(() => !IsPauseStateMachine);

			state = stateBase;
			await state.OnEnter();

			if (state.IsWaitForOnExit)
			{
				await UniTask.WaitUntil(() => !IsPauseStateMachine);
				await state.OnExit();
			}
		}

		public virtual Type GetStateType<T>(T type) where T : Enum { return null; }

		protected virtual UniTask SwitchToState(Type type) { return UniTask.CompletedTask; }
	}
}