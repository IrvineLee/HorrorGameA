using UnityEngine;
using System;

using Helper;
using Personal.Quest;

namespace Personal.Save
{
	[Serializable]
	public class PlayerSavedData
	{
		[SerializeField] int slotID = 0;

		[SerializeField] int characterID = 0;

		[SerializeField] SerializableDictionary<QuestType, QuestInfo> activeMainQuestDictionary = new();
		[SerializeField] SerializableDictionary<QuestType, QuestInfo> activeSubQuestDictionary = new();

		[SerializeField] SerializableDictionary<QuestType, QuestInfo> completedMainQuestDictionary = new();
		[SerializeField] SerializableDictionary<QuestType, QuestInfo> completedSubQuestDictionary = new();

		public int SlotID { get => slotID; set => slotID = value; }
		public int CharacterID { get => characterID; set => characterID = value; }

		public SerializableDictionary<QuestType, QuestInfo> ActiveMainQuestDictionary { get => activeMainQuestDictionary; set => activeMainQuestDictionary = value; }
		public SerializableDictionary<QuestType, QuestInfo> ActiveSubQuestDictionary { get => activeSubQuestDictionary; set => activeSubQuestDictionary = value; }
		public SerializableDictionary<QuestType, QuestInfo> CompletedMainQuestDictionary { get => completedMainQuestDictionary; set => completedMainQuestDictionary = value; }
		public SerializableDictionary<QuestType, QuestInfo> CompletedSubQuestDictionary { get => completedSubQuestDictionary; set => completedSubQuestDictionary = value; }
	}
}