
using Personal.Manager;
using UnityEngine;

namespace Personal.Quest
{
	public class QuestTypeSet : MonoBehaviour
	{
		[SerializeField] QuestType questType = QuestType.Main001_CallFather;

		public QuestType QuestType { get => questType; }

		public void TryToInitializeQuest()
		{
			QuestManager.Instance.TryUpdateData(questType);
		}
	}
}