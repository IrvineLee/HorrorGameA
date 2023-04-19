
namespace Personal.FSM.Cashier
{
	public class CashierStateBase : State
	{
		protected CashierStateMachine stateMachine;

		public CashierStateBase(CashierStateMachine stateMachine)
		{
			this.stateMachine = stateMachine;
		}
	}
}