using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;
using Personal.FSM;

namespace Personal.Quest
{
	public class QuestTypeSet : GameInitialize
	{
		[SerializeField] protected QuestType questType = QuestType._None;

		public QuestType QuestType { get => questType; }

		StateBase stateBase;

		protected override void Initialize()
		{
			stateBase = GetComponentInChildren<StateBase>();
			if (stateBase) stateBase.OnEnterEvent += TryUpdateData;
		}

		public void TryUpdateData()
		{
			QuestManager.Instance.TryUpdateData(questType).Forget();
		}

		void OnDestroy()
		{
			if (!stateBase) return;
			stateBase.OnEnterEvent -= TryUpdateData;
		}
	}
}