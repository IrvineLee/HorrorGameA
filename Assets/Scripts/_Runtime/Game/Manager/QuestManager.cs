using System.Collections.Generic;
using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Quest;
using Personal.Save;

namespace Personal.Manager
{
	public class QuestManager : GameInitializeSingleton<QuestManager>
	{
		SerializableDictionary<QuestType, QuestInfo> activeMainQuestDictionary = new();
		SerializableDictionary<QuestType, QuestInfo> activeSubQuestDictionary = new();

		SerializableDictionary<QuestType, QuestInfo> completedMainQuestDictionary = new();
		SerializableDictionary<QuestType, QuestInfo> completedSubQuestDictionary = new();

		PlayerSavedData playerSavedData;

		protected override void Initialize()
		{
			playerSavedData = GameStateBehaviour.Instance.SaveObject.PlayerSavedData;

			//activeMainQuestDictionary.Add(QuestType.Main001_CallFather, new QuestInfo("AAA"));
			//activeMainQuestDictionary.Add(QuestType.Sub001_CallCoin, new QuestInfo("BBB"));
			//activeMainQuestDictionary.Add(QuestType.Sub999_Test, new QuestInfo("CCC"));
		}

		public void UpdateSaveData()
		{
			playerSavedData.ActiveMainQuestDictionary = activeMainQuestDictionary;
			playerSavedData.ActiveSubQuestDictionary = activeSubQuestDictionary;

			playerSavedData.CompletedMainQuestDictionary = completedMainQuestDictionary;
			playerSavedData.CompletedSubQuestDictionary = completedSubQuestDictionary;
		}
	}
}