using UnityEngine;

using Personal.FSM;
using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler : InteractableObjectEventStateChange
	{
		bool isTriggerable = true;

		public void SetIsTriggerable(bool isFlag) { isTriggerable = isFlag; }

		async void OnTriggerEnter(Collider other)
		{
			if (!isTriggerable) return;

			var questSet = GetComponentInChildren<QuestTypeSet>();
			if (questSet)
			{
				QuestType questType = questSet.QuestType;
				if (!QuestManager.Instance.IsAbleToStartQuest(questType)) return;
			}

			// Disable the trigger collider.
			currentCollider.enabled = false;

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();
			await base.HandleInteraction();
		}
	}
}

