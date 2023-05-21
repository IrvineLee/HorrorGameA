using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class StateBase : MonoBehaviour
	{
		protected StateMachineBase stateMachine;

		bool isEntered;

		public void SetFSM(StateMachineBase stateMachine)
		{
			this.stateMachine = stateMachine;
		}

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask OnEnter()
		{
			await UniTask.Delay(0);
			isEntered = true;
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
		public virtual async UniTask OnExit()
		{
			await UniTask.Delay(0);
			isEntered = false;
		}
	}
}