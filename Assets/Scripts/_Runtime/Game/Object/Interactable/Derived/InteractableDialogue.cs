﻿using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM.Character;
using Personal.Character.NPC;
using Personal.Manager;
using Cinemachine;
using Personal.FSM;

namespace Personal.InteractiveObject
{
	public class InteractableDialogue : InteractableObject
	{
		protected HeadModelLookAt headModelLookAt;

		protected override void Initialize()
		{
			base.Initialize();

			headModelLookAt = GetComponentInChildren<HeadModelLookAt>(true);
		}

		protected override async UniTask HandleInteraction()
		{
			await HandleDialogue();
			await base.HandleInteraction();
		}

		/// <summary>
		/// Handle dialogue talking with interactables.
		/// </summary>
		async UniTask HandleDialogue()
		{
			dialogueSystemTrigger.OnUse(transform);

			// Enable LookAt state
			var fsm = StageManager.Instance.PlayerController.FSM;
			fsm.IFSMHandler.OnBegin(typeof(PlayerLookAtState));

			headModelLookAt?.SetLookAtTarget(true);

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => ((PlayerLookAtState)fsm.CurrentState).IsStateEnded, cancellationToken: this.GetCancellationTokenOnDestroy());

			// Enable POV control state.
			SetRotationToPOVControl();
			fsm.IFSMHandler.OnBegin(typeof(PlayerPOVControlState));

			await StageManager.Instance.DialogueController.WaitDialogueEnd();

			// Switch back to default standard state.
			headModelLookAt?.SetLookAtTarget(false);
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

