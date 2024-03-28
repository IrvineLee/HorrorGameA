
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using PixelCrushers.DialogueSystem;
using Cysharp.Threading.Tasks;
using Personal.Item;
using Personal.Manager;
using Personal.Character.Player;

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
			public bool IsSuccess { get; private set; }

			/// <summary>
			/// Whether the quest can still be progressed.
			/// </summary>
			public bool IsEnded { get; private set; }

			int questID;

			PlayerInventory playerInventory;
			ItemType itemType;

			public TaskInfo(int questID, string description, ActionType actionType, int objectiveKey, int requiredAmount)
			{
				this.questID = questID;

				this.description = description;
				this.actionType = actionType;
				this.objectiveKey = objectiveKey;
				this.requiredAmount = requiredAmount;

				if (actionType == ActionType.None)
				{
					IsSuccess = true;
					CloseTask();
				}

				if (!(IsCountable() && MasterDataManager.Instance.GetEnumType<Enum>(objectiveKey).GetType() == typeof(ItemType))) return;

				itemType = (ItemType)objectiveKey;

				if (actionType == ActionType.Acquire)
				{
					playerInventory = StageManager.Instance.PlayerController.Inventory;
					playerInventory.OnPickupItemEvent += AddToProgress;
				}
			}

			public void SetProgress(int value)
			{
				if (IsSuccess) return;

				progress = value;

				if (progress < 0 ||
					(IsCountable() && progress >= requiredAmount))
				{
					IsSuccess = true;
					CloseTask();
				}
			}

			public void CloseTask()
			{
				IsEnded = true;
			}

			public string GetProgressOverRequiredAmount()
			{
				if (!IsCountable()) return "";
				return " " + progress.ToString() + " / " + requiredAmount.ToString();
			}

			bool IsCountable()
			{
				return (actionType == ActionType.Kill || actionType == ActionType.Acquire || actionType == ActionType.Use);
			}

			void AddToProgress(PlayerInventory.Item item)
			{
				if (itemType != item.ItemType) return;
				progress = playerInventory.GetItemCount(itemType);

				UIManager.Instance.MainDisplayHandlerUI.UpdateQuestTasks(questID);
			}
		}

		[SerializeField] QuestState questState = QuestState.Active;

		public QuestEntity QuestEntity { get; private set; }
		public List<TaskInfo> TaskInfoList { get => taskInfoList; }
		public bool IsQuestSuccess { get => questState == QuestState.Completed; }
		public bool IsQuestEnded { get => questState == QuestState.Completed || questState == QuestState.Failed; }

		List<TaskInfo> taskInfoList = new List<TaskInfo>();

		public QuestInfo(QuestEntity questEntity)
		{
			QuestEntity = questEntity;

			TaskInfo taskInfo01 = new TaskInfo(questEntity.id, QuestEntity.taskDescription01, QuestEntity.taskActionType01, QuestEntity.taskObjectiveKey01, QuestEntity.taskRequiredAmount01);
			TaskInfo taskInfo02 = new TaskInfo(questEntity.id, QuestEntity.taskDescription02, QuestEntity.taskActionType02, QuestEntity.taskObjectiveKey02, QuestEntity.taskRequiredAmount02);
			TaskInfo taskInfo03 = new TaskInfo(questEntity.id, QuestEntity.taskDescription03, QuestEntity.taskActionType03, QuestEntity.taskObjectiveKey03, QuestEntity.taskRequiredAmount03);

			taskInfoList.Add(taskInfo01);
			taskInfoList.Add(taskInfo02);
			taskInfoList.Add(taskInfo03);
		}

		public void SetQuestState(QuestState questState) { this.questState = questState; }

		/// <summary>
		/// Update task for DialogueResponse/Acquire
		/// </summary>
		public async UniTask<QuestInfo> UpdateQuest(CancellationToken cancellationToken)
		{
			foreach (var taskInfo in taskInfoList)
			{
				await UpdateTask(taskInfo, cancellationToken);
			}

			if (IsTasksEnded()) HandleQuestEnd();

			if (!QuestEntity.isHiddenQuest)
			{
				// Handle the UI.
				UIManager.Instance.MainDisplayHandlerUI.UpdateQuest(this);
			}

			return this;
		}

		async UniTask UpdateTask(TaskInfo taskInfo, CancellationToken cancellationToken)
		{
			if (taskInfo.IsSuccess) return;

			switch (taskInfo.ActionType)
			{
				case ActionType.DialogueResponse: await HandleActionTypeDialogueResponse(taskInfo, cancellationToken); break;
				case ActionType.Use: HandleActionTypeUse(taskInfo); break;
			}
		}

		async UniTask HandleActionTypeDialogueResponse(TaskInfo taskInfo, CancellationToken cancellationToken)
		{
			UIManager.Instance.MainDisplayHandlerUI.UpdateQuest(this);

			// Wait next frame for the dialogue manager to set it's conversation id.
			await UniTask.NextFrame();

			if (taskInfo.ObjectiveKey != DialogueManager.Instance.LastConversationID) return;

			// Wait until the conversation is finished.
			await UniTask.WaitUntil(() => DialogueManager.Instance?.isConversationActive == false, cancellationToken: cancellationToken);

			var dialogueController = StageManager.Instance.DialogueController;
			int selectedResponse = dialogueController.DialogueSetup.DialogueResponseListHandler.SelectedResponse;

			if (taskInfo.RequiredAmount == selectedResponse)
			{
				taskInfo.SetProgress(-1);
			}
			taskInfo.CloseTask();

			// TODO: Complete show UI.
			Debug.Log("Update quest " + taskInfo.RequiredAmount + "   " + selectedResponse);
		}

		void HandleActionTypeUse(TaskInfo taskInfo)
		{
			Enum enumType = MasterDataManager.Instance.GetEnumType<Enum>(taskInfo.ObjectiveKey);
			if (enumType.GetType() == typeof(ItemType)) taskInfo.SetProgress(GlossaryManager.Instance.GetUsedType(enumType));

			// TODO: Complete show UI.
		}

		/// <summary>
		/// Check to see whether the quest has ended.
		/// </summary>
		/// <returns></returns>
		bool IsTasksEnded()
		{
			foreach (var taskInfo in taskInfoList)
			{
				if (!taskInfo.IsEnded) return false;
			}

			return true;
		}

		/// <summary>
		/// Handle quest complete/failed.
		/// </summary>
		/// <returns></returns>
		void HandleQuestEnd()
		{
			foreach (var taskInfo in taskInfoList)
			{
				if (!taskInfo.IsSuccess)
				{
					questState = QuestState.Failed;
					return;
				}
			}

			questState = QuestState.Completed;
		}
	}
}