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
using Cysharp.Threading.Tasks;

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
		/// See whether the quest has ended.
		/// </summary>
		/// <returns></returns>
		public bool IsQuestEnded(QuestType questType)
		{
			RefreshActiveAndEndedQuest(questType);
			return endedDictionary.ContainsKey(questType);
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
		/// End the quest. 
		/// </summary>
		/// <param name="questInfo"></param>
		public void TryEndQuest(QuestInfo questInfo)
		{
			if (questInfo.QuestState == QuestState.Completed || questInfo.QuestState == QuestState.Failed)
			{
				QuestType questType = (QuestType)questInfo.QuestEntity.id;

				activeDictionary.Remove(questType);
				endedDictionary.Add(questType, questInfo);
			}
		}

		/// <summary>
		/// Check to see whether it's able to update this quest.
		/// </summary>
		/// <param name="questType"></param>
		/// <returns></returns>
		bool IsAbleToUpdateData(QuestType questType)
		{
			RefreshActiveAndEndedQuest(questType);

			if (endedDictionary.ContainsKey(questType)) return false;
			if (!IsAbleToTriggerQuest(activeDictionary, questType)) return false;

			return true;
		}

		/// <summary>
		/// Get the correct dictionary for questType. Main quest or Sub quest dictionary.
		/// </summary>
		/// <param name="questType"></param>
		void RefreshActiveAndEndedQuest(QuestType questType)
		{
			activeDictionary = questData.ActiveMainQuestDictionary;
			endedDictionary = questData.EndedMainQuestDictionary;

			if (((int)questType).IsWithin(ConstantFixed.SUB_QUEST_START, ConstantFixed.SUB_QUEST_END))
			{
				activeDictionary = questData.ActiveSubQuestDictionary;
				endedDictionary = questData.EndedSubQuestDictionary;
			}
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
			questInfo.UpdateQuest().Forget();
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
	}
}