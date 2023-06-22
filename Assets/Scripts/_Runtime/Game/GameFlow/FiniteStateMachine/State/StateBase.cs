using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public abstract class StateBase : MonoBehaviour
	{
		[Tooltip("Should this state wait till OnExit finishes before proceeding to the next state?")]
		[SerializeField] bool isWaitForOnExit = false;

		public bool IsWaitForOnExit { get => isWaitForOnExit; }

		protected StateMachineBase stateMachine;
		protected bool isEntered;

		public void SetMyFSM(StateMachineBase stateMachine)
		{
			this.stateMachine = stateMachine;
		}

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnEnter()
		{
			isEntered = true;
			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Called from the state machine.
		/// </summary>
		/// <returns></returns>
		public void OnUpdateRun()
		{
			if (!isEntered) return;

			OnUpdate();
		}

		/// <summary>
		/// Called to request updating
		/// </summary>
		/// <returns></returns>
		public virtual void OnUpdate() { }

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnExit()
		{
			isEntered = false;
			return UniTask.CompletedTask;
		}

		/// <summary>
		/// This is typically used in CompareState script to halt the state before moving on.
		/// </summary>
		/// <returns></returns>
		public virtual UniTask CheckComparison() { return UniTask.CompletedTask; }
	}
}