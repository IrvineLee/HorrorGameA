using UnityEngine;

namespace Personal.FSM.Cashier
{
	public class CashierCounter : MonoBehaviour
	{
		[SerializeField] GameObject target = null;

		public GameObject Target { get => target; }
	}
}