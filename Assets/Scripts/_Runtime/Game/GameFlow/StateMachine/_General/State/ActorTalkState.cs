using System;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;

namespace Personal.FSM.Character
{
	[Serializable]
	public class ActorTalkState : ActorStateBase
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			var dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			dialogueSystemTrigger.OnUse(actorStateMachine.transform);

			RunActorAnimation();
			await UniTask.WaitUntil(() => DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);
		}
	}
}