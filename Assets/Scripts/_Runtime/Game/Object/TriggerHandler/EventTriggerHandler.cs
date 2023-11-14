using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler : InteractableEventBegin
	{
		void OnTriggerEnter(Collider other)
		{
			if (!IsInteractable) return;

			var questSet = GetComponentInChildren<QuestTypeSet>();
			if (questSet)
			{
				QuestType questType = questSet.QuestType;
				if (!QuestManager.Instance.IsAbleToStartQuest(questType)) return;
			}

			// Disable the trigger collider.
			colliderTrans.gameObject.SetActive(false);

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();
			base.HandleInteraction().Forget();
		}
	}
}

