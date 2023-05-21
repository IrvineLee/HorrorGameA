using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.FSM
{
	public abstract class StateMachineBase : GameInitialize
	{
		protected StateBase state;

		public async UniTask SetState(StateBase stateBase)
		{
			state?.OnExit().Forget();

			state = stateBase;
			if (stateBase == null) return;

			await state.OnEnter();
		}
	}
}