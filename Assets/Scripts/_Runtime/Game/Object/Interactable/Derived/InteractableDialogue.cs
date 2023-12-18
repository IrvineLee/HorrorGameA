using System;
using UnityEngine;

using PixelCrushers.DialogueSystem;
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

			headModelLookAt = GetComponentInChildren<HeadModelLookAt>();
		}

		protected override async UniTask HandleInteraction()
		{
			await HandleDialogue();
		}

		/// <summary>
		/// Handle dialogue talking with interactables.
		/// </summary>
		async UniTask HandleDialogue()
		{
			dialogueSystemTrigger.OnUse(transform);

			// Enable LookAt state
			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();
			ifsmHandler.OnBegin(typeof(PlayerLookAtState));

			headModelLookAt?.SetLookAtTarget(true);

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => !StageManager.Instance.CameraHandler.CinemachineBrain.IsBlending, cancellationToken: this.GetCancellationTokenOnDestroy());

			// Enable POV control state.
			SetRotationToPOVControl();
			InitiatorStateMachine.SwitchToState(typeof(PlayerPOVControlState)).Forget();

			await StageManager.Instance.DialogueController.WaitDialogueEnd();

			// Switch back to default standard state.
			headModelLookAt?.SetLookAtTarget(false);
			ifsmHandler.OnExit();
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

