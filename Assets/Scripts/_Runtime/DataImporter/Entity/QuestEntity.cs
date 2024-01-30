using System;
using Personal.Entity;
using Personal.Quest;

[Serializable]
public class QuestEntity : GenericNameEntity
{
	public bool isMainQuest;
	public bool isHiddenQuest;
	public int prerequisiteKey;

	public string taskDescription01;
	public ActionType taskActionType01;
	public int taskObjectiveKey01;
	public int taskRequiredAmount01;

	public string taskDescription02;
	public ActionType taskActionType02;
	public int taskObjectiveKey02;
	public int taskRequiredAmount02;

	public string taskDescription03;
	public ActionType taskActionType03;
	public int taskObjectiveKey03;
	public int taskRequiredAmount03;

	public int rewardKey01;
	public int rewardAmount01;

	public int rewardKey02;
	public int rewardAmount02;

	public int rewardKey03;
	public int rewardAmount03;
}
