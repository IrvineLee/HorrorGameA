
using System;
using System.Collections.Generic;
using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.Item;
using Personal.Manager;
using Cysharp.Threading.Tasks;

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
		[Serializable]
		public class TaskInfo
		{
			[SerializeField] string description;
			[SerializeField] ActionType actionType;
			[SerializeField] int objectiveKey;
			[SerializeField] int requiredAmount;

			// Current progress.
			[SerializeField] int progress;          // A value of -1 means it's completed. Other value are the total progress.

			public string Description { get => description; }
			public ActionType ActionType { get => actionType; }
			public int ObjectiveKey { get => objectiveKey; }
			public int RequiredAmount { get => requiredAmount; }

			public int Progress { get => progress; }

			/// <summary>
			/// Did the task complete successfully?
			/// </summary>
			public bool IsSuccess
			{
				get => actionType == ActionType.None || progress < 0 ||
					   (actionType != ActionType.DialogueResponse && progress >= requiredAmount);
			}

			/// <summary>
			/// Whether the quest can still be progressed.
			/// </summary>
			public bool IsEnded { get; private set; }

			public TaskInfo(string description, ActionType actionType, int objectiveKey, int requiredAmount)
			{
				this.description = description;
				this.actionType = actionType;
				this.objectiveKey = objectiveKey;
				this.requiredAmount = requiredAmount;
			}

			public void SetProgress(int value) { progress = value; }
			public void CloseTask() { IsEnded = true; }
		}

		[SerializeField] QuestState questState = QuestState.Active;

		public QuestEntity QuestEntity { get; private set; }
		public QuestState QuestState { get => questState; }
		public List<TaskInfo> TaskInfoList { get => taskInfoList; }
		public bool IsQuestEnded { get => QuestState == QuestState.Completed || QuestState == QuestState.Failed; }

		List<TaskInfo> taskInfoList = new List<TaskInfo>();

		public QuestInfo(QuestEntity questEntity)
		{
			QuestEntity = questEntity;

			TaskInfo taskInfo01 = new TaskInfo(QuestEntity.taskDescription01, QuestEntity.taskActionType01, QuestEntity.taskObjectiveKey01, QuestEntity.taskRequiredAmount01);
			TaskInfo taskInfo02 = new TaskInfo(QuestEntity.taskDescription02, QuestEntity.taskActionType02, QuestEntity.taskObjectiveKey02, QuestEntity.taskRequiredAmount02);
			TaskInfo taskInfo03 = new TaskInfo(QuestEntity.taskDescription03, QuestEntity.taskActionType03, QuestEntity.taskObjectiveKey03, QuestEntity.taskRequiredAmount03);

			taskInfoList.Add(taskInfo01);
			taskInfoList.Add(taskInfo02);
			taskInfoList.Add(taskInfo03);
		}

		public void SetQuestState(QuestState questState) { this.questState = questState; }

		/// <summary>
		/// Update task for DialogueResponse/Acquire
		/// </summary>
		public async UniTask UpdateQuest()
		{
			foreach (var taskInfo in taskInfoList)
			{
				await UpdateTask(taskInfo);
			}

			if (IsQuestCompleted())
			{
				questState = QuestState.Completed;
				QuestManager.Instance.TryEndQuest(this);
			}

			// Handle the UI.
			UIManager.Instance.MainDisplayHandlerUI.UpdateQuest(this);
		}

		async UniTask UpdateTask(TaskInfo taskInfo)
		{
			if (taskInfo.IsSuccess) return;

			switch (taskInfo.ActionType)
			{
				case ActionType.DialogueResponse: await HandleActionTypeDialogueResponse(taskInfo); break;
				case ActionType.Acquire: HandleActionTypeAcquire(taskInfo); break;
				case ActionType.Use: HandleActionTypeUse(taskInfo); break;
			}
		}

		async UniTask HandleActionTypeDialogueResponse(TaskInfo taskInfo)
		{
			UIManager.Instance.MainDisplayHandlerUI.UpdateQuest(this);

			// Wait next frame for the dialogue manager to set it's conversation id.
			await UniTask.NextFrame();

			if (taskInfo.ObjectiveKey != DialogueManager.Instance.LastConversationID) return;

			// Wait until the conversation is finished.
			await UniTask.WaitUntil(() => DialogueManager.Instance?.isConversationActive == false);

			int selectedResponse = QuestManager.Instance.DialogueSetup.DialogueResponseListHandler.SelectedResponse;
			if (taskInfo.RequiredAmount == selectedResponse)
			{
				taskInfo.SetProgress(-1);
			}
			taskInfo.CloseTask();
		}

		void HandleActionTypeAcquire(TaskInfo taskInfo)
		{
			var playerInventory = StageManager.Instance.PlayerController.Inventory;
			int count = playerInventory.GetItemCount((ItemType)taskInfo.ObjectiveKey);

			taskInfo.SetProgress(count);
		}

		void HandleActionTypeUse(TaskInfo taskInfo)
		{
			Type enumType = MasterDataManager.Instance.GetEnumType(taskInfo.ObjectiveKey);

			if (enumType == typeof(ItemType))
			{
				ItemType itemType = (ItemType)taskInfo.ObjectiveKey;
				taskInfo.SetProgress(GlossaryManager.Instance.GetUsedType(itemType));
			}
		}

		/// <summary>
		/// Check to see whether the quest is completed.
		/// </summary>
		/// <returns></returns>
		bool IsQuestCompleted()
		{
			foreach (var taskInfo in taskInfoList)
			{
				if (!taskInfo.IsSuccess && !taskInfo.IsEnded) return false;
			}
			return true;
		}
	}
}