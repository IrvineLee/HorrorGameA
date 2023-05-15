using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class StateBase : MonoBehaviour
	{
		protected StateMachineBase stateMachine;

		public void SetFSM(StateMachineBase stateMachine)
		{
			this.stateMachine = stateMachine;
		}

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnEnter() { return UniTask.Delay(0); }

		/// <summary>
		/// Called to request updating
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnUpdate() { return UniTask.Delay(0); }

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public virtual UniTask OnExit() { return UniTask.Delay(0); }
	}
}