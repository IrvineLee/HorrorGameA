using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.FSM
{
	public abstract class StateMachineBase : GameInitialize
	{
		protected StateBase state;

		public async UniTask SetState(StateBase state)
		{
			this.state?.OnExit().Forget();

			this.state = state;
			if (state == null) return;

			await state.OnEnter();
		}
	}
}