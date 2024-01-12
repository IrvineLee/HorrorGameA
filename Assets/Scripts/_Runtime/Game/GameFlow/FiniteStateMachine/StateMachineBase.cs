using System;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.GameState;
using UnityEngine;

namespace Personal.FSM
{
	public abstract class StateMachineBase : GameInitialize
	{
		[ShowInInspector]
		public StateBase CurrentState { get => state; }
		public StateMachineBase InitiatorStateMachine { get; protected set; }
		public bool IsPauseStateMachine { get; protected set; }

		protected StateBase state;

		protected async UniTask SetState(StateBase stateBase)
		{
			if (state != null && !state.IsWaitForOnExit)
			{
				state.OnExit().Forget();
			}

			state = stateBase;
			await state.OnEnter();

			if (state.IsWaitForOnExit) await state.OnExit();
		}

		public virtual Type GetStateType<T>(T type) where T : Enum { return null; }
		public void PauseStateMachine(bool isFlag) { IsPauseStateMachine = isFlag; }
	}
}