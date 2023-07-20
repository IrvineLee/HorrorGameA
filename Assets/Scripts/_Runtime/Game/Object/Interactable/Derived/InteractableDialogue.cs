using System;
using UnityEngine;

using PixelCrushers.DialogueSystem;
using Cysharp.Threading.Tasks;
using Personal.FSM.Character;
using Personal.Character.NPC;
using Personal.Manager;
using Cinemachine;

namespace Personal.InteractiveObject
{
	public class InteractableDialogue : InteractableObject
	{
		protected DialogueSystemTrigger dialogueSystemTrigger;
		protected HeadModelLookAt headModelLookAt;

		protected override void Awake()
		{
			base.Awake();

			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			headModelLookAt = GetComponentInChildren<HeadModelLookAt>();
		}

		protected override async UniTask HandleInteraction()
		{
			await HandleDialogue();
		}

		/// <summary>
		/// Handle dialogue talking with interactables.
		/// </summary>
		/// <param name="ifsmHandler"></param>
		/// <returns></returns>
		async UniTask HandleDialogue()
		{
			dialogueSystemTrigger.OnUse(transform);

			// Enable LookAt state
			InitiatorStateMachine.SwitchToState(typeof(PlayerLookAtState)).Forget();
			headModelLookAt?.SetLookAtTarget(true);

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => !StageManager.Instance.CinemachineBrain.IsBlending);

			// Enable POV control state.
			SetRotationToPOVControl();
			InitiatorStateMachine.SwitchToState(typeof(PlayerPOVControlState)).Forget();

			await UniTask.WaitUntil(() => DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);

			// Switch back to default standard state.
			headModelLookAt?.SetLookAtTarget(false);
			InitiatorStateMachine.SwitchToState(typeof(PlayerStandardState)).Forget();

			InitiatorStateMachine.SetLookAtTarget(null);
		}

		void SetRotationToPOVControl()
		{
			var stateDictionary = ((PlayerStateMachine)InitiatorStateMachine).StateDictionary;

			// Get lookAtState virtual camera.
			stateDictionary.TryGetValue(typeof(PlayerLookAtState), out var lookAtState);
			var vCam = lookAtState.GetComponentInChildren<CinemachineVirtualCamera>();

			// Replace the rotation onto the POV control state.
			stateDictionary.TryGetValue(typeof(PlayerPOVControlState), out var povControlState);
			povControlState.transform.rotation = vCam.transform.rotation;
		}
	}
}

