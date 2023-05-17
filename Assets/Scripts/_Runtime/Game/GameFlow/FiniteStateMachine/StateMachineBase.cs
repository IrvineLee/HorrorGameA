using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.FSM
{
	public abstract class StateMachineBase : GameInitialize
	{
		protected StateBase state;
		protected StateBase prevState;

		public async UniTask SetState(StateBase state)
		{
			if (prevState != null)
				await prevState.OnExit();

			if (state == null) return;

			this.state = state;
			prevState = state;

			await state.OnEnter();
		}
	}
}