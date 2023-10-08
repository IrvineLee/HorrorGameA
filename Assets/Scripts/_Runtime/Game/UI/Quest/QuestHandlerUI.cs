using System.Collections.Generic;
using UnityEngine;

using Personal.Quest;

namespace Personal.UI.Quest
{
	public class QuestHandlerUI : MonoBehaviour
	{
		[SerializeField] QuestContainerUI questContainerPrefab = null;

		Dictionary<int, QuestContainerUI> questContainerDictionary = new();

		public void SetQuest(QuestInfo questInfo)
		{
			int id = questInfo.QuestEntity.id;
			if (!questContainerDictionary.TryGetValue(id, out QuestContainerUI questContainerUI))
			{
				questContainerUI = Instantiate(questContainerPrefab, transform);
				questContainerDictionary.Add(id, questContainerUI);
			}

			questContainerUI.ShowQuest(questInfo);
			ArrangeQuest();
		}

		void ArrangeQuest()
		{
			foreach (var questContainer in questContainerDictionary)
			{
				if (!questContainer.Value.IsMainQuest) continue;
				questContainer.Value.transform.SetAsFirstSibling();
			}
		}
	}
}