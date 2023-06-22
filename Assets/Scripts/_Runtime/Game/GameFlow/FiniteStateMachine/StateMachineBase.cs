using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.FSM
{
	public abstract class StateMachineBase : GameInitialize
	{
		public StateBase CurrentState { get => state; }
		public StateMachineBase InitiatorStateMachine { get; protected set; }

		protected StateBase state;

		public async UniTask SetState(StateBase stateBase)
		{
			if (state != null)
			{
				if (state.IsWaitForOnExit) await state.OnExit();
				else state.OnExit().Forget();
			}

			state = stateBase;
			await state.OnEnter();
		}
	}
}