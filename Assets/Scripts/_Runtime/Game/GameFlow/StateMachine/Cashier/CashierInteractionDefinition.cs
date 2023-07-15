using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Personal.FSM;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "CashierInteraction", menuName = "ScriptableObjects/State/CashierInteraction", order = 0)]
	[Serializable]
	public class CashierInteractionDefinition : ScriptableObject
	{
		[SerializeField] string interactionResourcePath = "";

		Dictionary<string, InteractionAssign> interactionDictionary = new Dictionary<string, InteractionAssign>();

		/// <summary>
		/// Initialize data into dictionary.
		/// </summary>
		public void Initalize()
		{
			List<InteractionAssign> interactionList = Resources.LoadAll(interactionResourcePath, typeof(InteractionAssign)).Cast<InteractionAssign>().ToList();

			interactionDictionary.Clear();
			foreach (var interaction in interactionList)
			{
				interactionDictionary.Add(interaction.name, interaction);
			}
		}

		/// <summary>
		/// Get interaction by name.
		/// </summary>
		/// <param name="interactionStr"></param>
		/// <returns></returns>
		public InteractionAssign GetInteraction(string interactionStr)
		{
			return interactionDictionary.GetValueOrDefault(interactionStr);
		}
	}
}