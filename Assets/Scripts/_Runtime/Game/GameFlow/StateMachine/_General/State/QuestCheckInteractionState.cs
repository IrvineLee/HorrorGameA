using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Quest;
using Personal.Manager;

namespace Personal.FSM.Character
{
	[Serializable]
	public class QuestCheckInteractionState : StateBase
	{
		[SerializeField] List<QuestType> passedQuestTypeList = new();
		[SerializeField] InteractionAssign passedInteraction = null;
		[SerializeField] InteractionAssign failedInteraction = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			await UniTask.NextFrame();

			if (IsPassedQuest())
			{
				await PlayInteraction(passedInteraction);
				return;
			}

			await PlayInteraction(failedInteraction);
		}

		bool IsPassedQuest()
		{
			foreach (var quest in passedQuestTypeList)
			{
				if (!QuestManager.Instance.IsQuestPassed(quest)) return false;
			}
			return true;
		}

		async UniTask PlayInteraction(InteractionAssign interactionAssign)
		{
			foreach (var interaction in interactionAssign.OrderedStateList)
			{
				await interaction.OnEnter();
			}
		}
	}
}