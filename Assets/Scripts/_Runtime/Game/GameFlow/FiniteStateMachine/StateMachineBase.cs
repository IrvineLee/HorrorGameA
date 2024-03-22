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

		/// <summary>
		/// This begins the interactionAssign.
		/// </summary>
		/// <param name="interactionAssign">Most initiator is itself, but in certain cases, it might be the player/other NPCs.</param>
		/// <returns></returns>
		public virtual UniTask Begin(InteractionAssign interactionAssign, StateMachineBase initiatorFSM = null) { return UniTask.CompletedTask; }

		public virtual Type GetStateType<T>(T type) where T : Enum { return null; }

		protected virtual UniTask SwitchToState(Type type) { return UniTask.CompletedTask; }

		/// <summary>
		/// Makes sure that the final state of OnExit still gets called even when IsWaitForOnExit is not set.
		/// </summary>
		/// <returns></returns>
		protected async UniTask PlayEndState() { await state.OnExit(); }

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