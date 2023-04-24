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
		[SerializeField] List<CashierInteraction> customerInterctionList = new List<CashierInteraction>();

		Dictionary<string, CashierInteraction> customerInteractionDictionary = new Dictionary<string, CashierInteraction>();

		public void Initalize()
		{
			customerInteractionDictionary.Clear();
			foreach (var interaction in customerInterctionList)
			{
				customerInteractionDictionary.Add(interaction.name, interaction);
			}
		}

		public CashierInteraction GetInteraction(string interactionStr)
		{
			return customerInteractionDictionary.GetValueOrDefault(interactionStr);
		}
	}
}