using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Personal.FSM.Cashier;
using Sirenix.OdinInspector;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "CashierInteraction", menuName = "ScriptableObjects/State/CashierInteraction", order = 0)]
	[Serializable]
	public class CashierInteractionDefinition : ScriptableObject
	{
		[FolderPath(ParentFolder = "Assets/Prefabs/Resources/")]
		[SerializeField] string cashierInteractionResourcePath = "";

		Dictionary<string, CashierInteraction> customerInteractionDictionary = new Dictionary<string, CashierInteraction>();

		/// <summary>
		/// Initialize data into dictionary.
		/// </summary>
		public void Initalize()
		{
			List<CashierInteraction> interactionList = Resources.LoadAll(cashierInteractionResourcePath, typeof(CashierInteraction)).Cast<CashierInteraction>().ToList();

			customerInteractionDictionary.Clear();
			foreach (var interaction in interactionList)
			{
				customerInteractionDictionary.Add(interaction.name, interaction);
			}
		}

		/// <summary>
		/// Get interaction by name.
		/// </summary>
		/// <param name="interactionStr"></param>
		/// <returns></returns>
		public CashierInteraction GetInteraction(string interactionStr)
		{
			return customerInteractionDictionary.GetValueOrDefault(interactionStr);
		}
	}
}