using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Quest;
using Personal.Manager;

namespace Personal.FSM.Character
{
	[Serializable]
	public class QuestCheckTalkState : StateBase
	{
		[SerializeField] List<QuestType> passedQuestTypeList = new();
		[SerializeField] ActorTalkState passedTalkState = null;
		[SerializeField] ActorTalkState failedTalkState = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			if (IsPassedQuest())
			{
				await passedTalkState.OnEnter();
				return;
			}

			await failedTalkState.OnEnter();
		}

		bool IsPassedQuest()
		{
			foreach (var quest in passedQuestTypeList)
			{
				if (!QuestManager.Instance.IsQuestEnded(quest)) return false;
			}
			return true;
		}
	}
}