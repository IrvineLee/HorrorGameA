using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class StateBase : MonoBehaviour
	{
		protected StateMachineBase stateMachine;

		UniTask uniTask = new UniTask();

		public void SetFSM(StateMachineBase stateMachine)
		{
			this.stateMachine = stateMachine;
		}

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnEnter() { return uniTask; }

		/// <summary>
		/// Called to request updating
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnUpdate() { return uniTask; }

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnExit() { return uniTask; }
	}
}