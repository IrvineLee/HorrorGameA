using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	public abstract class PlayerBaseState : ActorStateBase
	{
		protected PlayerStateMachine playerFSM;

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public override UniTask OnEnter()
		{
			base.OnEnter();
			playerFSM = (PlayerStateMachine)stateMachine;

			return UniTask.CompletedTask;
		}
	}
}