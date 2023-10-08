using System.Collections.Generic;
using UnityEngine;

using Personal.Quest;
using Personal.Manager;

namespace Personal.UI.Quest
{
	public class QuestHandlerUI : MonoBehaviour
	{
		[SerializeField] QuestContainerUI questContainerPrefab = null;

		Dictionary<int, QuestContainerUI> questContainerDictionary = new();

		public void UpdateQuest(QuestInfo questInfo)
		{
			int id = questInfo.QuestEntity.id;
			if (!questContainerDictionary.TryGetValue(id, out QuestContainerUI questContainerUI))
			{
				questContainerUI = PoolManager.Instance.GetSpawnedObject(questContainerPrefab.name)?.GetComponentInChildren<QuestContainerUI>();
				if (!questContainerUI) questContainerUI = Instantiate(questContainerPrefab, transform);

				questContainerDictionary.Add(id, questContainerUI);
			}

			if (questInfo.IsQuestEnded)
			{
				questContainerUI.ResetText();

				PoolManager.Instance.ReturnSpawnedObject(questContainerUI.gameObject);
				questContainerDictionary.Remove(id);

				return;
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