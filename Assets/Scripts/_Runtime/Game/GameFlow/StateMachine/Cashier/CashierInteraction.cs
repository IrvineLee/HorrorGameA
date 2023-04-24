using System.Collections.Generic;
using UnityEngine;

namespace Personal.FSM.Cashier
{
	public class CashierInteraction : MonoBehaviour
	{
		public List<StateBase> OrderedStateList
		{
			get
			{
				if (orderedStateList.Count != 0)
					return orderedStateList;

				foreach (Transform child in transform)
				{
					orderedStateList.Add(child.GetComponent<StateBase>());
				}
				return orderedStateList;
			}
		}

		List<StateBase> orderedStateList = new List<StateBase>();
	}
}