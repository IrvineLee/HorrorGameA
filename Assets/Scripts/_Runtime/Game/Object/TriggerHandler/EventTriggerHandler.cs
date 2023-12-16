using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler : InteractableEventBegin
	{
		protected List<Collider> allColliderList = new();

		protected override void Initialize()
		{
			base.Initialize();

			allColliderList = GetComponentsInChildren<Collider>().ToList();
		}

		void OnTriggerEnter(Collider other)
		{
			if (!IsInteractable) return;

			var questSet = GetComponentInChildren<QuestTypeSet>();
			if (questSet)
			{
				QuestType questType = questSet.QuestType;
				if (!QuestManager.Instance.IsAbleToStartQuest(questType)) return;
			}

			// Disable all the trigger collider.
			foreach (var collider in allColliderList)
			{
				collider.enabled = false;
			}

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();
			base.HandleInteraction().Forget();
		}
	}
}

