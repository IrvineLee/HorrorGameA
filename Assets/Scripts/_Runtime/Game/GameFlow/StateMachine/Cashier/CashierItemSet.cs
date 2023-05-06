using UnityEngine;

namespace Personal.FSM.Cashier
{
	public class CashierItemSet : MonoBehaviour
	{
		[SerializeField] CashierItemType cashierItemType = CashierItemType.ItemParent_Day01_01;

		public CashierItemType CashierItemType { get => cashierItemType; }
	}
}