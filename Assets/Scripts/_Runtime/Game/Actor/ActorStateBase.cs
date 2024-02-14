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
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			actorStateMachine = (ActorStateMachine)stateMachine;
		}

		/// <summary>
		/// Called when the state ended. The controller themselves should set their own state back.
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnExit()
		{
			await base.OnExit();
		}

		protected virtual void HandleMovement() { }

		protected virtual void RunActorAnimation()
		{
			actorStateMachine.AnimatorController?.PlayAnimation(actorAnimationType);
		}
	}
}