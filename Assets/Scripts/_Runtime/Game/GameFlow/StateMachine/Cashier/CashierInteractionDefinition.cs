using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.FSM.Cashier;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "CashierInteraction", menuName = "ScriptableObjects/State/CashierInteraction", order = 0)]
	[Serializable]
	public class CashierInteractionDefinition : ScriptableObject
	{
		[SerializeReference] List<CashierInteraction> customerInterctionList = new List<CashierInteraction>();

		public List<CashierInteraction> StateList { get => customerInterctionList; }
	}
}