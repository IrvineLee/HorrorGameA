
namespace Personal.Quest
{
	public enum QuestState
	{
		Undiscovered = 0,       // Undiscovered
		Discovered,             // Active quest where the player is able to see in quest log
		Active,                 // Active quest that is selected to be shown in the main UI (pinned quest)
		Completed,              // Completed quest
		Failed,                 // Failed quest
	}
}