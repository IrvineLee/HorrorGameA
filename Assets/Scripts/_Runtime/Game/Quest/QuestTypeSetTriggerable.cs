using UnityEngine;

using Personal.Manager;
using Personal.InteractiveObject;

namespace Personal.Quest
{
	public class QuestTypeSetTriggerable : QuestTypeSet
	{
		[SerializeField] EventTriggerHandler eventTriggerHandler = null;

		protected override void Initialize()
		{
			bool isEnded = QuestManager.Instance.IsQuestEnded(questType);
			if (isEnded && eventTriggerHandler) eventTriggerHandler.SetIsTriggerable(false);
		}
	}
}