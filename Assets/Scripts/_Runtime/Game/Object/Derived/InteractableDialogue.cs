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

		public override async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast = default)
		{
			var ifsmHandler = stateMachineBase.GetComponentInChildren<IFSMHandler>();
			await HandleDialogue(ifsmHandler);

			doLast?.Invoke();
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

			await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive);

			headLookAt.SetLookAtTarget(false);
			ifSMHandler?.OnExit();
		}
	}
}

