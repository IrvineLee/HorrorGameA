using Personal.GameFlow.CashierSystem;

namespace Personal.FSM
{
	public class CashierState : State
	{
		protected CashierSystem cashierSystem;

		public CashierState(CashierSystem cashierSystem)
		{
			this.cashierSystem = cashierSystem;
		}
	}
}