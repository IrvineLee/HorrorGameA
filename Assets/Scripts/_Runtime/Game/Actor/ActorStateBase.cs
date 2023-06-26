using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character.Animation;

namespace Personal.FSM.Character
{
	public class ActorStateBase : StateBase
	{
		[SerializeField] ActorAnimationType actorAnimationType = ActorAnimationType.None;

		protected ActorStateMachine actorStateMachine;

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public override UniTask OnEnter()
		{
			base.OnEnter();
			actorStateMachine = (ActorStateMachine)stateMachine;

			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public override UniTask OnExit()
		{
			base.OnExit();
			return UniTask.CompletedTask;
		}

		protected virtual void HandleMovement() { }

		protected virtual void RunActorAnimation()
		{
			actorStateMachine.AnimatorController?.PlayAnimation(actorAnimationType);
		}
	}
}