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
using Personal.KeyEvent;

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
			if (interactableState == InteractableState.EndRemainInteractable || interactableState == InteractableState.EndDialogue)
			{
				SetIsInteractable(true);
			}
		}

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			InitiatorStateMachine = initiatorStateMachine;

			if (!isInteractable) return;

			if (interactableState == InteractableState.EndDialogue && IsDefinitionHasFlag(InteractableCompleteType.EndDialogue))
			{
				await HandleInteractionDialogue(InteractableType.EndDialogue, interactDefinition.EndedDialogue, doLast);
				return;
			}

			if (await IsState(InteractableState.Examinable, InteractableType.ExaminableBeforeKeyEvent, doLast)) return;
			else if (await IsState(InteractableState.Requirement, InteractableType.Requirement, doLast)) return;

			await HandleAchieveRequirement_BeforeInteract();
			await HandleInteraction();
			await HandleAchieveRequirement_AfterInteract();
			await HandleGetReward();

			doLast?.Invoke();
		}

		async UniTask<bool> IsState(InteractableState state, InteractableType interactableType, Action doLast)
		{
			if (!interactDefinition) return false;

			string dialogue = interactDefinition.ExaminableDialogue;
			if (interactableType == InteractableType.Requirement) dialogue = interactDefinition.RequiredItemDialogue;

			if (interactableState == state && IsDefinitionHasFlag(interactableType))
			{
				await HandleInteractionDialogue(interactableType, dialogue, doLast);
				return true;
			}
			return false;
		}

		public void ShowOutline(bool isFlag)
		{
			outlinableFadeInOut?.StartFade(isFlag);
		}

		protected virtual UniTask HandleInteraction()
		{
			if (IsCompleteInteraction()) interactableState = InteractableState.CompleteInteraction;
			return UniTask.CompletedTask;
		}

		protected virtual bool IsCompleteInteraction() { return true; }

		protected virtual bool HasRequiredItems()
		{
			foreach (var itemData in interactDefinition.RequiredItemTypeList)
			{
				if (StageManager.Instance.PlayerController.Inventory.GetItemCount(itemData.ItemType) <= 0) return false;
			}

			return true;
		}

		protected virtual async UniTask HandleGetReward()
		{
			if (interactableState != InteractableState.CompleteInteraction) return;
			if (interactableState == InteractableState.EndRemainInteractable) return;

			if (IsDefinitionHasFlag(InteractableType.Reward))
			{
				await HandleInteractionDialogue(InteractableType.Reward, interactDefinition.RewardDialogue);
			}

			HandleEnd();
		}

		/// <summary>
		/// This checks whether the correct InteractionType is selected before playing the dialogue.
		/// </summary>
		protected async UniTask HandleInteractionDialogue(InteractableType interactableType, string dialogue, Action doLast = default)
		{
			if (!interactDefinition) return;
			if (interactDefinition.InteractionType.HasFlag(interactableType) == false) return;
			if (!dialogueSystemTrigger) { Debug.LogWarning(name + " has no Dialogue System Trigger"); return; }

			dialogueSystemTrigger.conversation = dialogue;
			dialogueSystemTrigger?.OnUse(InitiatorStateMachine.transform);

			await StageManager.Instance.DialogueController.WaitDialogueEnd();
			doLast?.Invoke();
		}

		async UniTask HandleAchieveRequirement_BeforeInteract()
		{
			if (!interactDefinition) return;
			if (interactableState == InteractableState.Interactable_OnlyOnceFinished) return;

			Action doLast = () =>
			{
				if (interactDefinition.IsOnlyOnce_BeforeUse) interactableState = InteractableState.Interactable_OnlyOnceFinished;
			};

			await HandleAnimationAndDialogue(
				InteractableAnimatorType.BeforeInteract_BeforeTalk,
				InteractableType.AchieveRequirement_BeforeInteract,
				interactDefinition.AchievedRequiredBeforeUseDialogue,
				InteractableAnimatorType.BeforeInteract_AfterTalk,
				doLast);
		}

		async UniTask HandleAchieveRequirement_AfterInteract()
		{
			if (!interactDefinition) return;

			await HandleAnimationAndDialogue(
				InteractableAnimatorType.AfterInteract_BeforeTalk,
				InteractableType.AchieveRequirement_AfterInteract,
				interactDefinition.AchievedRequiredAfterUseDialogue,
				InteractableAnimatorType.AfterInteract_AfterTalk);
		}

		async UniTask HandleAnimationAndDialogue(InteractableAnimatorType startAnimatorType, InteractableType interactableType,
			string dialogue, InteractableAnimatorType endAnimatorType, Action doLast = default)
		{
			if (interactableState == InteractableState.EndRemainInteractable) return;

			await HandleAnimator(startAnimatorType);
			await HandleInteractionDialogue(interactableType, dialogue);
			await HandleAnimator(endAnimatorType);

			doLast?.Invoke();
		}

		protected void HandleEnd()
		{
			interactableState = InteractableState.EndNonInteractable;
			if (interactDefinition)
			{
				StageManager.Instance.GetReward(interactDefinition.RewardItemList).Forget();

				if (interactDefinition.InteractableCompleteType == InteractableCompleteType.EndDialogue)
				{
					interactableState = InteractableState.EndDialogue;
				}
				else if (interactDefinition.InteractableCompleteType == InteractableCompleteType.RemainInteractable)
				{
					interactableState = InteractableState.EndRemainInteractable;
				}

				StageManager.Instance.RegisterKeyEvent(interactDefinition.CompleteKeyEventType);
			}

			SetIsInteractable(interactableState == InteractableState.EndRemainInteractable || interactableState == InteractableState.EndDialogue);
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
			if (!animator.gameObject.activeInHierarchy) return;

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

