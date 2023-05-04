using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM.Cashier
{
	public class CashierCustomerLeaveEnd : StateBase
	{
		public override async UniTask OnEnter()
		{
			Debug.Log("Customer leave END!!");
			await UniTask.DelayFrame(0);
			return;
		}

		public override async UniTask OnUpdate()
		{
			await UniTask.DelayFrame(0);
		}

		public override async UniTask OnExit()
		{
			await UniTask.DelayFrame(0);
		}
	}
}