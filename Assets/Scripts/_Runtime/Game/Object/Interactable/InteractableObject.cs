using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using Helper;
using Personal.FSM;
using Personal.Definition;
using Personal.GameState;
using Personal.Manager;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Personal.InteractiveObject
{
	public abstract class InteractableObject : GameInitialize
	{
		// This is a unique ID for saving/loading objects in scene.
		[SerializeField] [ReadOnly] protected string id;

		[SerializeField] protected Transform colliderTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		[SerializeField] bool isInteractable = true;
		[SerializeField] InteractableDialogueDefinition interactDialogueDefinition = null;

		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine InitiatorStateMachine { get; protected set; }
		public bool IsInteractable { get => isInteractable; }

		protected MeshRenderer meshRenderer;
		protected OutlinableFadeInOut outlinableFadeInOut;

		protected DialogueSystemTrigger dialogueSystemTrigger;

		// This handles the save/load state of this object.
		protected bool isEndExaminableDialogue;
		protected bool isOnlyOnce_BeforeInteractEnded;
		protected bool isMainInteractionCompleted;                      // Will be true after passing the reward phase(even when there is no reward).
		protected bool isInteractionEnded;

		protected override void Initialize()
		{
			meshRenderer = GetComponentInChildren<MeshRenderer>(true);
			if (colliderTrans) outlinableFadeInOut = colliderTrans.GetComponentInChildren<OutlinableFadeInOut>(true);

			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			SetIsInteractable(isInteractable);

			if (interactDialogueDefinition == null || !IsDefinitionHasFlag(InteractableType.ExaminableBeforeKeyEvent))
			{
				isEndExaminableDialogue = true;
			}
		}

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			InitiatorStateMachine = initiatorStateMachine;

			if (!isInteractable) return;

			Transform actor = initiatorStateMachine.transform;
			if (!isEndExaminableDialogue)
			{
				await HandleInteractionDialogue(InteractableType.ExaminableBeforeKeyEvent, interactDialogueDefinition.ExaminableDialogue, actor, doLast);
				return;
			}

			if (!isMainInteractionCompleted && !HasRequiredItems() && IsDefinitionHasFlag(InteractableType.Reward))
			{
				await HandleInteractionDialogue(InteractableType.Requirement, interactDialogueDefinition.RequiredItemDialogue, actor, doLast);
				return;
			}
			else if (isMainInteractionCompleted && IsDefinitionHasFlag(InteractableCompleteType.EndDialogue))
			{
				await HandleInteractionDialogue(InteractableType.End, interactDialogueDefinition.EndedDialogue, actor, doLast);
				return;
			}

			await HandleAchieveRequirement_BeforeInteract(actor);
			await HandleInteraction();
			await HandleAchieveRequirement_AfterInteract(actor);
			await HandleGetReward(actor);

			doLast?.Invoke();
		}

		public void ShowOutline(bool isFlag)
		{
			outlinableFadeInOut?.StartFade(isFlag);
		}

		public void SetIsInteractable(bool isFlag)
		{
			isInteractable = isFlag;
			if (colliderTrans) colliderTrans.gameObject.SetActive(isFlag);
		}

		/// <summary>
		/// This only handles for the flag of InteractableType.ExaminableBeforeKeyEvent. After calling this, it will no longer enter that dialogue.
		/// Nothing happens if the flag is not selected.
		/// </summary>
		public void EndExaminableDialogue()
		{
			isEndExaminableDialogue = true;
		}

		protected virtual async UniTask HandleInteraction() { await UniTask.CompletedTask; }

		protected virtual bool HasRequiredItems()
		{
			// Return if it's not Requirement.
			if (!IsDefinitionHasFlag(InteractableType.Requirement)) return true;

			foreach (var item in interactDialogueDefinition.RequiredItemTypeList)
			{
				if (StageManager.Instance.PlayerController.Inventory.GetItemCount(item) <= 0) return false;
			}

			return true;
		}

		protected virtual async UniTask HandleGetReward(Transform actor)
		{
			if (IsDefinitionHasFlag(InteractableType.Reward))
			{
				await HandleInteractionDialogue(InteractableType.Reward, interactDialogueDefinition.RewardDialogue, actor);
			}

			StageManager.Instance.GetReward(interactDialogueDefinition.RewardInteractableObjectList).Forget();

			isMainInteractionCompleted = true;
			if (interactDialogueDefinition == null || IsDefinitionHasFlag(InteractableCompleteType.NotInteractable)) SetIsInteractable(false);
		}

		/// <summary>
		/// This checks whether the correct InteractionType is selected before playing the dialogue.
		/// </summary>
		protected async UniTask HandleInteractionDialogue(InteractableType interactableType, string dialogue, Transform actor, Action doLast = default)
		{
			if (!dialogueSystemTrigger) return;
			if (!interactDialogueDefinition) return;
			if (interactDialogueDefinition.InteractableType.HasFlag(interactableType) == false) return;

			dialogueSystemTrigger.conversation = dialogue;
			dialogueSystemTrigger?.OnUse(actor);

			await StageManager.Instance.DialogueController.WaitDialogueEnd();
			doLast?.Invoke();
		}

		protected async UniTask HandleAchieveRequirement_BeforeInteract(Transform actor)
		{
			if (!interactDialogueDefinition) return;

			bool isOnlyOnce = interactDialogueDefinition.IsOnlyOnce_BeforeUse;
			if ((isOnlyOnce && !isOnlyOnce_BeforeInteractEnded) || !isOnlyOnce)
			{
				await HandleInteractionDialogue(InteractableType.AchieveRequirement_BeforeInteract, interactDialogueDefinition.AchievedRequiredBeforeUseDialogue, actor);
			}

			// TODO : Supposedly an animation/sound effect or a wait period to show to player what is happening. Probably in another class .Play().
			await UniTask.Delay(0, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
			if (interactDialogueDefinition.IsOnlyOnce_BeforeUse == true) isOnlyOnce_BeforeInteractEnded = true;
		}

		protected async UniTask HandleAchieveRequirement_AfterInteract(Transform actor)
		{
			if (!interactDialogueDefinition) return;

			// TODO : Supposedly an animation/sound effect after finishing the interaction. Probably in another class .Play().
			await UniTask.Delay(0, cancellationToken: gameObject.GetCancellationTokenOnDestroy());

			await HandleInteractionDialogue(InteractableType.AchieveRequirement_AfterInteract, interactDialogueDefinition.AchievedRequiredAfterUseDialogue, actor);
		}

		bool IsDefinitionHasFlag(InteractableType interactableType)
		{
			return (interactDialogueDefinition != null && interactDialogueDefinition.InteractableType.HasFlag(interactableType) == true);
		}

		bool IsDefinitionHasFlag(InteractableCompleteType interactableCompleteType)
		{
			return (interactDialogueDefinition != null && interactDialogueDefinition.InteractableCompleteType.HasFlag(interactableCompleteType) == true);
		}

		[ContextMenu("GenerateGUID")]
		void GenerateGUID()
		{
			StringHelper.GenerateNewGuid(ref id);
		}

		[ContextMenu("ResetGUID")]
		void ResetGUID()
		{
			id = "";
		}

#if UNITY_EDITOR
		void OnValidate()
		{
			if (PrefabUtility.IsPartOfPrefabAsset(gameObject)) return;
			if (string.IsNullOrEmpty(id) || gameObject.name.IsDuplicatedGameObject()) GenerateGUID();
		}
#endif
	}
}

