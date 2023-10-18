using System.Collections.Generic;
using UnityEngine;

using Personal.Quest;
using Personal.Manager;
using Personal.GameState;

namespace Personal.InteractiveObject
{
	public class OnInteractionEnd : GameInitialize
	{
		[SerializeField] List<InteractableObject> interactableObjectList = new();

		QuestTypeSet questTypeSet;

		protected override void Initialize()
		{
			questTypeSet = GetComponentInChildren<QuestTypeSet>();
		}

		public void EnableInteractables()
		{
			bool isEnded = questTypeSet == null ? true : QuestManager.Instance.IsQuestEnded(questTypeSet.QuestType);
			if (isEnded && interactableObjectList.Count > 0)
			{
				foreach (var interactable in interactableObjectList)
				{
					interactable.SetIsInteractable(true);
				}
			}
		}
	}
}

