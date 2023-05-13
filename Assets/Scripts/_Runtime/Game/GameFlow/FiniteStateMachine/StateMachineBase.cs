using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.FSM
{
	public abstract class StateMachineBase : GameInitialize
	{
		protected StateBase state;

		public async UniTask SetState(StateBase state)
		{
			this.state = state;
			await state.OnEnter();
		}
	}
}