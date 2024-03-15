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
		bool isUpdate;

		protected async UniTask SetState(StateBase stateBase)
		{
			isUpdate = false;

			await HandlePreviousState();
			await HandlePauseFSM();

			state = stateBase;

			var enterState = state.OnEnter();
			isUpdate = true;

			await enterState;

			if (state.IsWaitForOnExit)
			{
				await HandlePauseFSM();
				await state.OnExit();
			}
		}

		protected void Update()
		{
			if (state == null) return;
			if (IsPauseStateMachine) return;
			if (!isUpdate) return;

			state.OnUpdate();
		}

		public virtual Type GetStateType<T>(T type) where T : Enum { return null; }

		protected virtual UniTask SwitchToState(Type type) { return UniTask.CompletedTask; }

		/// <summary>
		/// Handle previous state. 
		/// </summary>
		/// <returns></returns>
		async UniTask HandlePreviousState()
		{
			if (state == null) return;
			if (state.IsWaitForOnExit) return;

			await HandlePauseFSM();
			state.OnExit().Forget();
		}

		async UniTask HandlePauseFSM()
		{
			if (IsPauseStateMachine) await UniTask.WaitUntil(() => !IsPauseStateMachine, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
		}
	}
}