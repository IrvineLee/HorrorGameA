using System;
using Personal.Entity;
using Personal.Quest;

[Serializable]
public class QuestEntity : GenericEntity
{
	public string key;
	public string name;
	public bool isMainQuest;
	public bool isDisplayQuest;
	public int prerequisiteKey;

	public string description01;
	public ActionType actionType01;
	public int requiredKey01;
	public int requiredAmount01;

	public string description02;
	public ActionType actionType02;
	public int requiredKey02;
	public int requiredAmount02;

	public int rewardKey;
	public int rewardAmount;
}
