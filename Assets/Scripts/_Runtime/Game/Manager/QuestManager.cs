using System;
using UnityEngine;

using Helper;
using Personal.GameState;
using Personal.Quest;
using Personal.Save;
using System.Collections.Generic;

namespace Personal.Manager
{
	public class QuestManager : GameInitializeSingleton<QuestManager>
	{
		// CAUTION : REMEMBER TO CHANGE MAX_TASK VALUE ACCORDING TO THE NUMBER OF TASKS IN THE MASTERQUEST.
		public const int MAX_TASK = 3;

		public const int SUB_QUEST_INDEX = 25000;
		public const int END_QUEST_INDEX = 29999;

		SerializableDictionary<QuestType, QuestInfo> activeDictionary = new();
		SerializableDictionary<QuestType, QuestInfo> endedDictionary = new();

		QuestData questData;

		protected override void Initialize()
		{
			questData = GameStateBehaviour.Instance.SaveObject.PlayerSavedData.QuestData;
		}

		/// <summary>
		/// This should be called from gameobject with quest ID that has task of ActionType.DialogueResponse/Acquire.
		/// </summary>
		/// <param name="questType"></param>
		public void TryUpdateData(QuestType questType)
		{
			if (!IsAbleToUpdateData(questType)) return;

			UpdateData(questType);
		}

		/// <summary>
		/// Check to see whether it's able to update this quest.
		/// </summary>
		/// <param name="questType"></param>
		/// <returns></returns>
		bool IsAbleToUpdateData(QuestType questType)
		{
			activeDictionary = questData.ActiveMainQuestDictionary;
			endedDictionary = questData.EndedMainQuestDictionary;

			if (((int)questType).IsWithin(SUB_QUEST_INDEX, SUB_QUEST_INDEX))
			{
				activeDictionary = questData.ActiveSubQuestDictionary;
				endedDictionary = questData.EndedSubQuestDictionary;
			}

			if (endedDictionary.ContainsKey(questType)) return false;
			if (!IsAbleToTriggerQuest(activeDictionary, questType)) return false;

			return true;
		}

		/// <summary>
		/// Check to see whether the prerequisite is met.
		/// </summary>
		/// <param name="activeDictionary"></param>
		/// <param name="questType"></param>
		/// <returns></returns>
		bool IsAbleToTriggerQuest(Dictionary<QuestType, QuestInfo> activeDictionary, QuestType questType)
		{
			QuestEntity entity = MasterDataManager.Instance.Quest.Get((int)questType);

			if (activeDictionary.ContainsKey((QuestType)entity.prerequisiteKey)) return true;
			return false;
		}

		void UpdateData(QuestType questType)
		{
			// Update the quest.
			QuestInfo questInfo = GetQuestInfo(questType);
			questInfo.UpdateQuest();

			CheckQuestCompletion(questType, questInfo);
		}

		QuestInfo GetQuestInfo(QuestType questType)
		{
			// Add the quest to the active dictionary.
			if (!activeDictionary.TryGetValue(questType, out QuestInfo questInfo))
			{
				QuestEntity entity = MasterDataManager.Instance.Quest.Get((int)questType);
				questInfo = new QuestInfo(entity);
				questInfo.SetQuestState(QuestState.Discovered);

				activeDictionary.Add(questType, questInfo);
			}
			return questInfo;
		}

		void CheckQuestCompletion(QuestType questType, QuestInfo questInfo)
		{
			questInfo.CheckCompletion();

			// End the quest.
			if (questInfo.QuestState == QuestState.Completed || questInfo.QuestState == QuestState.Failed)
			{
				activeDictionary.Remove(questType);
				endedDictionary.Add(questType, questInfo);
			}
		}
	}
}