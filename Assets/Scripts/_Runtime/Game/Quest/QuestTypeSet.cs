using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;
using Personal.InteractiveObject;

namespace Personal.Quest
{
	public class QuestTypeSet : GameInitialize
	{
		[SerializeField] protected QuestType questType = QuestType._20000_Main001_CallFather;

		public QuestType QuestType { get => questType; }

		OnInteractionEnd onInteractionEnded;

		protected override void Initialize()
		{
			onInteractionEnded = GetComponentInChildren<OnInteractionEnd>();
		}

		public async UniTask TryToInitializeQuest()
		{
			await QuestManager.Instance.TryUpdateData(questType);
			onInteractionEnded?.EnableInteractables();
		}
	}
}