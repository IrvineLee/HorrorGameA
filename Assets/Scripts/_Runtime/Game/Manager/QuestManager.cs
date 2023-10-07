using System.Collections.Generic;
using UnityEngine;

using Helper;
using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Quest;
using Personal.Save;
using Personal.Constant;
using Personal.Dialogue;
using QuestState = Personal.Quest.QuestState;

namespace Personal.Manager
{
	public class QuestManager : GameInitializeSingleton<QuestManager>
	{
		public DialogueSetup DialogueSetup { get => dialogueSetup; }

		SerializableDictionary<QuestType, QuestInfo> activeDictionary = new();
		SerializableDictionary<QuestType, QuestInfo> endedDictionary = new();

		DialogueSetup dialogueSetup;
		QuestData questData;

		protected override void Initialize()
		{
			dialogueSetup = DialogueManager.Instance.GetComponentInChildren<DialogueSetup>();
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

			if (((int)questType).IsWithin(ConstantFixed.SUB_QUEST_START, ConstantFixed.SUB_QUEST_END))
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

			if (entity.prerequisiteKey == 0) return true;
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