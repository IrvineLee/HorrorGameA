using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Personal.FSM.Cashier
{
	public class CashierInteraction : MonoBehaviour
	{
		[SerializeField] List<StateBase> orderedStateList = new List<StateBase>();

		public List<StateBase> OrderedStateList { get => orderedStateList; }

		[Button("Initialize", Icon = SdfIconType.Dice6Fill, IconAlignment = IconAlignment.RightEdge, Stretch = false, ButtonAlignment = 1f)]
		void Initialize()
		{
			orderedStateList.Clear();
			foreach (Transform child in transform)
			{
				orderedStateList.Add(child.GetComponent<StateBase>());
			}
		}
	}
}