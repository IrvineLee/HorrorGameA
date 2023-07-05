using System;
using UnityEngine;

using PixelCrushers.DialogueSystem;
using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.FSM.Character;
using Personal.Character.NPC;
using Personal.Manager;
using Helper;

namespace Personal.InteractiveObject
{
	public class InteractableDialogue : InteractableObject
	{
		protected DialogueSystemTrigger dialogueSystemTrigger;
		protected HeadModelLookAt headModelLookAt;

		protected Camera cam;

		CoroutineRun lookAtCR = new CoroutineRun();
		Type initiatorType;

		protected override void Awake()
		{
			base.Awake();

			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			headModelLookAt = GetComponentInChildren<HeadModelLookAt>();

			cam = StageManager.Instance.MainCamera;
		}

		protected override async UniTask HandleInteraction()
		{
			if (InitiatorStateMachine.GetType() == typeof(PlayerStateMachine))
			{
				initiatorType = typeof(PlayerLookAtState);
			}

			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();
			await HandleDialogue(ifsmHandler);
		}

		/// <summary>
		/// Handle only dialogue talking with interactables.
		/// </summary>
		/// <param name="ifsmHandler"></param>
		/// <returns></returns>
		async UniTask HandleDialogue(IFSMHandler ifsmHandler)
		{
			dialogueSystemTrigger.OnUse(transform);

			ifsmHandler?.OnBegin(initiatorType);
			headModelLookAt?.SetLookAtTarget(true);

			await UniTask.WaitUntil(() => lookAtCR.IsDone && DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);

			headModelLookAt?.SetLookAtTarget(false);
			ifsmHandler?.OnExit();
		}
	}
}

