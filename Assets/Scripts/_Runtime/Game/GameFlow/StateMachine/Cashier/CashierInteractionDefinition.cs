using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "CashierInteraction", menuName = "ScriptableObjects/State/CashierInteraction", order = 0)]
	[Serializable]
	public class CashierInteractionDefinition : ScriptableObject
	{
		[SerializeField] string interactionResourcePath = "";

		Dictionary<string, Transform> interactionPrefabDictionary = new Dictionary<string, Transform>();

		/// <summary>
		/// Initialize data into dictionary.
		/// </summary>
		public void Initalize()
		{
			var interactionPrefabList = Resources.LoadAll(interactionResourcePath, typeof(Transform)).Cast<Transform>().ToList();

			interactionPrefabDictionary.Clear();
			foreach (var interaction in interactionPrefabList)
			{
				interactionPrefabDictionary.Add(interaction.name, interaction);
			}
		}

		/// <summary>
		/// Get interaction by name.
		/// </summary>
		/// <param name="interactionStr"></param>
		/// <returns></returns>
		public Transform GetInteraction(string interactionStr)
		{
			return interactionPrefabDictionary.GetValueOrDefault(interactionStr);
		}
	}
}