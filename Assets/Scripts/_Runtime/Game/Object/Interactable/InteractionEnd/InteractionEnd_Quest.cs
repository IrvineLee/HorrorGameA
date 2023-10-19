using System.Collections.Generic;
using UnityEngine;

using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_Quest : InteractionEnd
	{
		[SerializeField] QuestTypeSet questTypeSet = null;

		protected override bool IsEnded()
		{
			return QuestManager.Instance.IsQuestEnded(questTypeSet.QuestType);
		}
	}
}

