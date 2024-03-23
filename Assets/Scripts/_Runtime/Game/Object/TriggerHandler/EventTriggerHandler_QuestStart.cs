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
		protected override async UniTask<bool> HandleTrigger()
		{
			var questSet = interactionAssign.FSM.GetComponentInChildren<QuestTypeSet>();
			if (!questSet) return false;

			QuestType questType = questSet.QuestType;
			return await QuestManager.Instance.TryUpdateData(questType);
		}
	}
}

