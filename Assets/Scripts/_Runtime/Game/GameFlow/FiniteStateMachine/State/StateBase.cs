using System;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public abstract class StateBase : MonoBehaviour
	{
		[Tooltip("Should this state wait till OnExit finishes before proceeding to the next state?")]
		[SerializeField] bool isWaitForOnExit = false;

		public bool IsWaitForOnExit { get => isWaitForOnExit; }

		public event Action OnEnterEvent;
		public event Action OnExitEvent;

		protected StateMachineBase stateMachine;

		void Awake()
		{
			stateMachine = GetComponentInParent<StateMachineBase>(true);
		}

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnEnter()
		{
			OnEnterEvent?.Invoke();

			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Called when the state updates
		/// </summary>
		/// <returns></returns>
		public virtual void OnUpdate() { }

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnExit()
		{
			OnExitEvent?.Invoke();

			return UniTask.CompletedTask;
		}

		/// <summary>
		/// This is typically used in CompareState script to halt the state before moving on.
		/// Can also be used to reset values, similar to OnExit but at different time.
		/// </summary>
		/// <returns></returns>
		public virtual UniTask Standby() { return UniTask.CompletedTask; }

		private void OnValidate()
		{
			transform.name = GetType().Name;
		}

		void Reset()
		{
			transform.name = GetType().Name;
		}
	}
}