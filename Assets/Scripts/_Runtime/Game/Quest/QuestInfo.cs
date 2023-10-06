
using System;
using System.Collections.Generic;

using PixelCrushers.DialogueSystem;
using Personal.Item;
using Personal.Manager;

namespace Personal.Quest
{
	[Serializable]
	/// <summary>
	/// CAUTION : 
	/// As of now, once a task is over, that task will forever be finished and there is no way to re-evaluate it again. (TODO in the future)
	/// So for example, if you collected 3 items and is considered finished, but you discarded them away, it will still be considered finished.
	/// There might be some cases where you still need to use it to finish the quest but you can't anymore, which could result in soft-locking the game.
	/// </summary>
	public class QuestInfo
	{
		public QuestEntity QuestEntity { get; private set; }
		public QuestState QuestState { get; private set; }
		public List<int> ProgressList { get => progressList; }        // A value of -1 means it's completed. Other value are the total count.

		List<int> progressList = new();

		public QuestInfo(QuestEntity questEntity)
		{
			QuestEntity = questEntity;

			for (int i = 0; i < QuestManager.MAX_TASK; i++)
			{
				progressList.Add(0);
			}

			UpdateAllTask();
		}

		public void SetQuestState(QuestState questState) { QuestState = questState; }

		public void UpdateQuest()
		{
			UpdateAllTask();
		}

		/// <summary>
		/// Update task after using specific item.
		/// </summary>
		/// <param name="useType"></param>
		public void UpdateUseTask<T>(T useType) where T : Enum
		{
			progressList[0] = UpdateUseTask(progressList[0], QuestEntity.taskActionType01, QuestEntity.taskObjectiveKey01, QuestEntity.taskRequiredAmount01, useType);
			progressList[1] = UpdateUseTask(progressList[1], QuestEntity.taskActionType02, QuestEntity.taskObjectiveKey02, QuestEntity.taskRequiredAmount02, useType);
			progressList[2] = UpdateUseTask(progressList[2], QuestEntity.taskActionType03, QuestEntity.taskObjectiveKey03, QuestEntity.taskRequiredAmount03, useType);
		}

		/// <summary>
		/// Used to check for quest completion.
		/// </summary>
		public void CheckCompletion()
		{
			if (IsQuestCompleted()) QuestState = QuestState.Completed;
		}

		void UpdateAllTask()
		{
			progressList[0] = UpdateTask(progressList[0], QuestEntity.taskActionType01, QuestEntity.taskObjectiveKey01, QuestEntity.taskRequiredAmount01);
			progressList[1] = UpdateTask(progressList[1], QuestEntity.taskActionType02, QuestEntity.taskObjectiveKey02, QuestEntity.taskRequiredAmount02);
			progressList[2] = UpdateTask(progressList[2], QuestEntity.taskActionType03, QuestEntity.taskObjectiveKey03, QuestEntity.taskRequiredAmount03);
		}

		int UpdateTask(int progress, ActionType actionType, int objectiveKey, int requiredAmount)
		{
			// Empty/completed tasks means it's completed.
			if (actionType == ActionType.None || progress <= -1) return -1;

			if (actionType == ActionType.DialogueResponse)
			{
				if (objectiveKey != DialogueManager.Instance.LastConversationID) return 0;
				return -1;
			}
			else if (actionType == ActionType.Acquire)
			{
				var playerInventory = StageManager.Instance.PlayerController.Inventory;
				int count = playerInventory.GetItemCount((ItemType)objectiveKey);

				if (requiredAmount >= count) return -1;
			}
			return 0;
		}

		int UpdateUseTask<T>(int progress, ActionType actionType, int objectiveKey, int requiredAmount, T useType) where T : Enum
		{
			if (actionType == ActionType.Use && objectiveKey == (int)(object)useType)
			{
				++progress;
				if (requiredAmount >= progress) return -1;
			}
			return progress;
		}

		/// <summary>
		/// Check to see whether the quest is completed.
		/// </summary>
		/// <returns></returns>
		bool IsQuestCompleted()
		{
			foreach (var progress in progressList)
			{
				if (progress >= 0) return false;
			}
			return true;
		}
	}
}