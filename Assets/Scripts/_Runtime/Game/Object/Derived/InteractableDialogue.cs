using System;

using PixelCrushers.DialogueSystem;
using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Character;

namespace Personal.Object
{
	public class InteractableDialogue : InteractableObject
	{
		protected DialogueSystemTrigger dialogueSystemTrigger;
		protected HeadLookAt headLookAt;

		protected override async UniTask Awake()
		{
			await base.Awake();
			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			headLookAt = GetComponentInChildren<HeadLookAt>();
		}

		protected override async UniTask HandleInteraction(ActorStateMachine actorStateMachine)
		{
			var ifsmHandler = actorStateMachine.GetComponentInChildren<IFSMHandler>();
			await HandleDialogue(ifsmHandler);
		}

		/// <summary>
		/// Handle only dialogue talking with interactables.
		/// </summary>
		/// <param name="ifSMHandler"></param>
		/// <returns></returns>
		async UniTask HandleDialogue(IFSMHandler ifSMHandler)
		{
			dialogueSystemTrigger.OnUse(transform);

			ifSMHandler?.OnBegin();
			headLookAt.SetLookAtTarget(true);

			await UniTask.WaitUntil(() => DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);

			headLookAt.SetLookAtTarget(false);
			ifSMHandler?.OnExit();
		}
	}
}

