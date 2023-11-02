using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.GameState;
using Personal.Quest;
using Personal.Save;
using Personal.Constant;
using QuestState = Personal.Quest.QuestState;

namespace Personal.Manager
{
	public class QuestManager : GameInitializeSingleton<QuestManager>
	{
		SerializableDictionary<QuestType, QuestInfo> activeDictionary = new();
		SerializableDictionary<QuestType, QuestInfo> endedDictionary = new();

		QuestData questData;

		protected override void Initialize()
		{
			questData = GameStateBehaviour.Instance.SaveObject.PlayerSavedData.QuestData;
		}

		/// <summary>
		/// Can you start this quest.
		/// </summary>
		/// <param name="questType"></param>
		/// <returns></returns>
		public bool IsAbleToStartQuest(QuestType questType)
		{
			if (IsQuestEnded(questType)) return false;
			if (!IsAbleToTriggerQuest(questType)) return false;

			return true;
		}

		/// <summary>
		/// See whether the quest passed successfully.
		/// </summary>
		/// <param name="questType"></param>
		/// <returns></returns>
		public bool IsQuestPassed(QuestType questType)
		{
			RefreshActiveAndEndedQuest(questType);

			if (!endedDictionary.ContainsKey(questType)) return false;

			return endedDictionary.GetOrDefault(questType).IsQuestSuccess;
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
			if (!IsAbleToStartQuest(questType)) return;

			UpdateData(questType);
		}

		/// <summary>
		/// End the quest and get reward. 
		/// </summary>
		/// <param name="questInfo"></param>
		public void TryGetReward(QuestInfo questInfo)
		{
			if (!questInfo.IsQuestEnded) return;

			var questEntity = questInfo.QuestEntity;

			// Reward.
			if (questInfo.IsQuestSuccess)
			{
				ObtainReward(questEntity.rewardKey01, questEntity.rewardAmount01);
				ObtainReward(questEntity.rewardKey02, questEntity.rewardAmount02);
				ObtainReward(questEntity.rewardKey03, questEntity.rewardAmount03);
			}

			QuestType questType = (QuestType)questEntity.id;

			activeDictionary.Remove(questType);
			endedDictionary.Add(questType, questInfo);
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
		bool IsAbleToTriggerQuest(QuestType questType)
		{
			QuestEntity entity = MasterDataManager.Instance.Quest.Get((int)questType);

			if (entity.prerequisiteKey == 0) return true;
			if (questData.EndedMainQuestDictionary.ContainsKey((QuestType)entity.prerequisiteKey) ||
				questData.EndedSubQuestDictionary.ContainsKey((QuestType)entity.prerequisiteKey)) return true;

			return false;
		}

		void UpdateData(QuestType questType)
		{
			// Update the quest.
			QuestInfo questInfo = GetQuestInfo(questType);
			questInfo.UpdateQuest(this.GetCancellationTokenOnDestroy()).Forget();
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

		void ObtainReward(int rewardKey, int rewardAmount)
		{
			if (rewardKey == 0) return;

			if (rewardKey < 0) questData.MainStoryPoints += rewardAmount;

			//Type type = MasterDataManager.Instance.GetEnumType(rewardKey);
			//if (type == typeof(ItemType))
			//{
			//}
		}
	}
}