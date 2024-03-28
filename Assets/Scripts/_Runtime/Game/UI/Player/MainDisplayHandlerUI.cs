using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Quest;
using Personal.UI.Quest;

namespace Personal.UI
{
	public class MainDisplayHandlerUI : MonoBehaviour
	{
		[SerializeField] QuestHandlerUI questHandler = null;

		public void UpdateQuest(QuestInfo questInfo)
		{
			questHandler.UpdateQuest(questInfo).Forget();
		}

		public void UpdateQuestTasks(int questID)
		{
			questHandler.UpdateQuestTasks(questID);
		}
	}
}

