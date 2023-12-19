using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler_Quest : EventTriggerHandler
	{
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

