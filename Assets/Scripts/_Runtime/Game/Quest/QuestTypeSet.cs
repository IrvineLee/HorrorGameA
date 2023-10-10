using UnityEngine;

using Personal.GameState;
using Personal.Manager;

namespace Personal.Quest
{
	public class QuestTypeSet : GameInitialize
	{
		[SerializeField] protected QuestType questType = QuestType.Main001_CallFather;

		public QuestType QuestType { get => questType; }

		public void TryToInitializeQuest()
		{
			QuestManager.Instance.TryUpdateData(questType);
		}
	}
}