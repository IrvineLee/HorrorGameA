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

namespace Personal.InteractiveObject
{
	public abstract class InteractableObject : GameInitialize
	{
		[SerializeField] [ReadOnly] protected string guid = Guid.NewGuid().ToString();

		[SerializeField] protected Transform colliderTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		[SerializeField] bool isInteractable = true;
		[SerializeField] InteractableDefinition interactDefinition = null;

		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine InitiatorStateMachine { get; protected set; }
		public bool IsInteractable { get => isInteractable; }

		protected MeshRenderer meshRenderer;
		protected OutlinableFadeInOut outlinableFadeInOut;

		protected DialogueSystemTrigger dialogueSystemTrigger;

		Animator animator;

		int befInteractBefTalkID;
		int befInteractAftTalkID;
		int aftInteractBefTalkID;
		int aftInteractAftTalkID;

		// This handles the save/load state of this object.
		protected InteractableState interactableState = InteractableState.Interactable;

		protected override void Initialize()
		{
			meshRenderer = GetComponentInChildren<MeshRenderer>(true);
			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();

			if (colliderTrans) outlinableFadeInOut = colliderTrans.GetComponentInChildren<OutlinableFadeInOut>(true);
			SetIsInteractable(isInteractable);

			InitAnimator();
			HandleExaminable();

			// Set the persistant data.
			if (interactableState == InteractableState.EndNonInteractable || interactableState == InteractableState.EndRemainInteractable)
			{
				SetIsInteractable(interactableState == InteractableState.EndRemainInteractable);
			}
		}

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			InitiatorStateMachine = initiatorStateMachine;

			if (!isInteractable) return;

			Transform actor = initiatorStateMachine.transform;
			if (interactableState == InteractableState.Examinable)
			{
				await HandleInteractionDialogue(InteractableType.ExaminableBeforeKeyEvent, interactDefinition.ExaminableDialogue, actor);
				doLast?.Invoke();
				return;
			}

			if (interactableState == InteractableState.Requirement && !HasRequiredItems() && IsDefinitionHasFlag(InteractableType.Requirement))
			{
				await HandleInteractionDialogue(InteractableType.Requirement, interactDefinition.RequiredItemDialogue, actor);
				doLast?.Invoke();
				return;
			}
			else if (interactableState == InteractableState.EndRemainInteractable && IsDefinitionHasFlag(InteractableCompleteType.EndDialogue))
			{
				await HandleInteractionDialogue(InteractableType.EndDialogue, interactDefinition.EndedDialogue, actor);
				doLast?.Invoke();
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

		protected virtual async UniTask HandleInteraction() { await UniTask.CompletedTask; }

		protected virtual bool HasRequiredItems()
		{
			// Return if it's not Requirement.
			if (!IsDefinitionHasFlag(InteractableType.Requirement)) return true;

			foreach (var itemData in interactDefinition.RequiredItemTypeList)
			{
				if (StageManager.Instance.PlayerController.Inventory.GetItemCount(itemData.ItemType) <= 0) return false;
			}

			return true;
		}

		protected virtual async UniTask HandleGetReward(Transform actor)
		{
			if (interactableState == InteractableState.EndRemainInteractable) return;

			if (IsDefinitionHasFlag(InteractableType.Reward))
			{
				await HandleInteractionDialogue(InteractableType.Reward, interactDefinition.RewardDialogue, actor);
			}

			HandleEnd();
		}

		/// <summary>
		/// This checks whether the correct InteractionType is selected before playing the dialogue.
		/// </summary>
		protected async UniTask HandleInteractionDialogue(InteractableType interactableType, string dialogue, Transform actor)
		{
			if (!interactDefinition) return;
			if (interactDefinition.InteractionType.HasFlag(interactableType) == false) return;
			if (!dialogueSystemTrigger) { Debug.LogWarning(name + " has no Dialogue System Trigger"); return; }

			dialogueSystemTrigger.conversation = dialogue;
			dialogueSystemTrigger?.OnUse(actor);

			await StageManager.Instance.DialogueController.WaitDialogueEnd();
		}

		protected async UniTask HandleAchieveRequirement_BeforeInteract(Transform actor)
		{
			if (!interactDefinition) return;
			if (interactableState == InteractableState.EndRemainInteractable) return;
			if (interactableState == InteractableState.Interactable_BeforeInteractFinished) return;

			await HandleAnimator(InteractableAnimatorType.BeforeInteract_BeforeTalk);
			await HandleInteractionDialogue(InteractableType.AchieveRequirement_BeforeInteract, interactDefinition.AchievedRequiredBeforeUseDialogue, actor);
			await HandleAnimator(InteractableAnimatorType.BeforeInteract_AfterTalk);

			if (interactDefinition.IsOnlyOnce_BeforeUse)
			{
				interactableState = InteractableState.Interactable_BeforeInteractFinished;
			}
		}

		protected async UniTask HandleAchieveRequirement_AfterInteract(Transform actor)
		{
			if (!interactDefinition) return;
			if (interactableState == InteractableState.EndRemainInteractable) return;

			await HandleAnimator(InteractableAnimatorType.AfterInteract_BeforeTalk);
			await HandleInteractionDialogue(InteractableType.AchieveRequirement_AfterInteract, interactDefinition.AchievedRequiredAfterUseDialogue, actor);
			await HandleAnimator(InteractableAnimatorType.AfterInteract_AfterTalk);
		}

		protected void HandleEnd()
		{
			interactableState = InteractableState.EndNonInteractable;
			if (interactDefinition)
			{
				StageManager.Instance.GetReward(interactDefinition.RewardItemList).Forget();

				if (interactDefinition.InteractableCompleteType == InteractableCompleteType.RemainInteractable)
				{
					interactableState = InteractableState.EndRemainInteractable;
				}

				StageManager.Instance.RegisterKeyEvent(interactDefinition.CompleteKeyEventType);
			}

			SetIsInteractable(interactableState == InteractableState.EndRemainInteractable);
		}

		void HandleExaminable()
		{
			bool isExaminable = IsDefinitionHasFlag(InteractableType.ExaminableBeforeKeyEvent);
			if (!isExaminable) return;

			interactableState = InteractableState.Examinable;
			StageManager.OnKeyEventCompleted += OnKeyEventCompleted;
		}

		bool IsDefinitionHasFlag<T>(T interactableType) where T : Enum
		{
			return (interactDefinition != null && interactDefinition.HasFlag(interactableType));
		}

		void OnKeyEventCompleted(KeyEventType keyEventType)
		{
			if (interactDefinition.KeyEventEndExaminableType != keyEventType) return;
			interactableState = InteractableState.Interactable;
		}

		void SetIsInteractable(bool isFlag)
		{
			isInteractable = isFlag;
			if (colliderTrans) colliderTrans.gameObject.SetActive(isFlag);
		}

		void InitAnimator()
		{
			if (!interactDefinition) return;
			animator = GetComponentInChildren<Animator>();

			befInteractBefTalkID = Animator.StringToHash(interactDefinition.BeforeInteractBeforeTalkParam);
			befInteractAftTalkID = Animator.StringToHash(interactDefinition.BeforeInteractAfterTalkParam);
			aftInteractBefTalkID = Animator.StringToHash(interactDefinition.AfterInteractBeforeTalkParam);
			aftInteractAftTalkID = Animator.StringToHash(interactDefinition.AfterInteractAfterTalkParam);
		}

		async UniTask HandleAnimator(InteractableAnimatorType interactableAnimatorType)
		{
			if (!animator) return;

			int hashID = GetHashID(interactableAnimatorType);
			if (hashID <= 0) return;

			animator.Play(hashID);
			bool isDone = false;

			CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => isDone = true);
			await UniTask.WaitUntil(() => isDone, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
		}

		int GetHashID(InteractableAnimatorType interactableAnimatorType)
		{
			switch (interactableAnimatorType)
			{
				case InteractableAnimatorType.BeforeInteract_BeforeTalk: return befInteractBefTalkID;
				case InteractableAnimatorType.BeforeInteract_AfterTalk: return befInteractAftTalkID;
				case InteractableAnimatorType.AfterInteract_BeforeTalk: return aftInteractBefTalkID;
				case InteractableAnimatorType.AfterInteract_AfterTalk: return aftInteractAftTalkID;
			}
			return -1;
		}

		void OnDestroy()
		{
			StageManager.OnKeyEventCompleted -= OnKeyEventCompleted;
		}

		void OnValidate()
		{
			if (string.IsNullOrEmpty(guid) || gameObject.name.IsDuplicatedGameObject())
			{
				name = name.SearchBehindRemoveFrontOrEnd('(', true);
				guid = Guid.NewGuid().ToString();
			}
		}
	}
}

