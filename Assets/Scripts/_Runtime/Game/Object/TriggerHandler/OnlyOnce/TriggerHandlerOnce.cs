using UnityEngine;

using Personal.Quest;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	/// <summary>
	/// This handles only 1 specific state without the FSM.
	/// </summary>
	public class TriggerHandlerOnce : MonoBehaviour
	{
		protected Collider currentCollider;
		protected bool isTriggerable = true;

		void Awake()
		{
			currentCollider = GetComponentInChildren<Collider>();
		}

		public void SetIsTriggerable(bool isFlag) { isTriggerable = isFlag; }

		protected virtual void OnTriggerEnter(Collider other) { }

		protected bool IsTriggerable()
		{
			if (!isTriggerable) return false;

			var questSet = GetComponentInChildren<QuestTypeSet>();
			if (questSet)
			{
				QuestType questType = questSet.QuestType;
				if (!QuestManager.Instance.IsAbleToStartQuest(questType)) return false;
			}
			return true;
		}
	}
}

