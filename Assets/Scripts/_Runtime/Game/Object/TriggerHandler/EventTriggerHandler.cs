﻿using UnityEngine;

using Personal.FSM;
using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler : InteractableObjectEventStateChange
	{
		async void OnTriggerEnter(Collider other)
		{
			if (!IsInteractable) return;

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
