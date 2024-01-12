using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler_QuestStart : EventTriggerHandler
	{
		/// <summary>
		/// Check to see whether you are able to start the quest.
		/// </summary>
		/// <returns></returns>
		protected override UniTask<bool> HandleTrigger()
		{
			var questSet = GetComponentInChildren<QuestTypeSet>();
			if (questSet)
			{
				QuestType questType = questSet.QuestType;
				if (!QuestManager.Instance.IsAbleToStartQuest(questType)) return new UniTask<bool>(false);
			}

			return new UniTask<bool>(true);
		}
	}
}

