using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using Helper;
using Personal.FSM;
using Personal.Definition;
using Personal.GameState;
using Personal.Manager;
using Personal.Item;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Personal.InteractiveObject
{
	public abstract class InteractableObject : GameInitialize
	{
		#region Constants
		const string EXAMINABLE_BEFORE_KEY_EVENT_STRING = "@interactionType.HasFlag(InteractableType.ExaminableBeforeKeyEvent)";

		const string REQUIRED_STRING = "@interactionType.HasFlag(InteractableType.Requirement)";
		const string ACHIEVED_REQUIRED_BEFORE_USE_STRING = "@interactionType.HasFlag(InteractableType.Requirement) && interactionType.HasFlag(InteractableType.AchieveRequirement_BeforeEvent)";
		const string ACHIEVED_REQUIRED_AFTER_USE_STRING = "@interactionType.HasFlag(InteractableType.Requirement) && interactionType.HasFlag(InteractableType.AchieveRequirement_AfterEvent)";

		const string REWARD_STRING = "@interactionType.HasFlag(InteractableType.Reward)";
		const string END_STRING = "@interactionType.HasFlag(InteractableType.End)";
		#endregion

		// This is a unique ID for saving/loading objects in scene.
		[SerializeField] [ReadOnly] protected string id;

		[SerializeField] protected Transform colliderTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		[SerializeField] bool isInteractable = true;

		[Tooltip("You only care about this when interactableType is ExaminableBeforeKeyEvent. If it is, you have to set this to true after the key event happened.")]
		[SerializeField] bool isActivatedKeyEvent = true;

		[SerializeField] InteractableType interactionType;

		#region Examinable

		[ShowIf(EXAMINABLE_BEFORE_KEY_EVENT_STRING)]
		[Header("Examinable")]
		[Tooltip("The dialogue when the key event has not enable this object to be interactable.")]
		[SerializeField] [ConversationPopup(true, true)] string examinableDialogue = null;

		#endregion

		#region Required Item

		[ShowIf(REQUIRED_STRING)]
		[Header("Requirement")]
		[Tooltip("The dialogue when the player does not have the required items to enable interaction.")]
		[SerializeField] [ConversationPopup(true, true)] string requiredItemDialogue = null;

		[ShowIf(REQUIRED_STRING)]
		[Tooltip("The item needed to enable/trigger the interaction." +
			" Some object might be interactable(pre-talk) but cannot be used/triggered/picked up until you get other items first.")]
		[SerializeField] List<ItemType> requiredItemTypeList = new();

		#endregion

		#region Achieved Required Before Use

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Header("Achived requirement, before use")]
		[Tooltip("Does it happen only once?")]
		[SerializeField] bool isOnlyOnceBeforeUse = false;

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Tooltip("The dialogue when you have the required item, but before using them.")]
		[SerializeField] [ConversationPopup(true, true)] string achievedRequiredBeforeUseDialogue = null;

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Tooltip("The wait duration after completing achievedRequiredBeforeUseDialogue.")]
		[SerializeField] float waitDurationComplete = 0f;

		#endregion

		#region Achieved Required After Use

		[ShowIf(ACHIEVED_REQUIRED_AFTER_USE_STRING)]
		[Header("Achived requirement, after use")]
		[Tooltip("The dialogue when you have the required item and after using them.")]
		[SerializeField] [ConversationPopup(true, true)] string achievedRequiredAfterUseDialogue = null;

		#endregion

		#region Reward

		[Header("Reward")]
		[ShowIf(REWARD_STRING)]
		[Tooltip("The interaction dialogue when you get the reward.")]
		[SerializeField] [ConversationPopup(true, true)] string rewardDialogue = null;

		#endregion

		#region End

		[ShowIf(END_STRING)]
		[Header("End")]
		[Tooltip("The next interaction dialogue after getting the reward. Null means it's not interactable anymore")]
		[SerializeField] [ConversationPopup(true, true)] string endedDialogue = null;

		[ShowIf(END_STRING)]
		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		#endregion

		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine InitiatorStateMachine { get; protected set; }
		public bool IsInteractable { get => isInteractable; }

		protected MeshRenderer meshRenderer;
		protected OutlinableFadeInOut outlinableFadeInOut;

		protected DialogueSystemTrigger dialogueSystemTrigger;
		protected bool isGottenReward;

		// This handles the save/load state of this object.
		protected bool isActivatedKeyEventEnded;
		protected bool isOnlyOnceBeforeUseEnded;
		protected bool isInteractionEnded;

		protected override void Initialize()
		{
			meshRenderer = GetComponentInChildren<MeshRenderer>(true);
			if (colliderTrans) outlinableFadeInOut = colliderTrans.GetComponentInChildren<OutlinableFadeInOut>(true);

			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			SetIsInteractable(isInteractable);
		}

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			if (!isInteractable) return;
			if (!isActivatedKeyEvent && !interactionType.HasFlag(InteractableType.ExaminableBeforeKeyEvent))
			{
				await SetDialogue(examinableDialogue, initiatorStateMachine.transform, doLast);
				return;
			}

			if (!HasRequiredItems())
			{
				await SetDialogue(requiredItemDialogue, initiatorStateMachine.transform, doLast);
				return;
			}
			else if (isGottenReward)
			{
				await SetDialogue(endedDialogue, initiatorStateMachine.transform, doLast);
				return;
			}

			InitiatorStateMachine = initiatorStateMachine;

			await HandleInteraction();
			await HandleGetReward(initiatorStateMachine);

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

		public void SetIsActivatedKeyEvent(bool isFlag)
		{
			isActivatedKeyEventEnded = isActivatedKeyEvent = isFlag;
		}

		protected virtual async UniTask HandleInteraction() { await UniTask.CompletedTask; }

		protected virtual bool HasRequiredItems()
		{
			// Return if it's not Requirement.
			if (!interactionType.HasFlag(InteractableType.Requirement)) return true;

			foreach (var item in requiredItemTypeList)
			{
				if (StageManager.Instance.PlayerController.Inventory.GetItemCount(item) <= 0) return false;
			}

			return true;
		}

		protected virtual async UniTask HandleGetReward(ActorStateMachine initiatorStateMachine)
		{
			if (!isOnlyOnceBeforeUseEnded && interactionType.HasFlag(InteractableType.AchieveRequirement_BeforeEvent))
			{
				await SetDialogue(achievedRequiredBeforeUseDialogue, initiatorStateMachine.transform);
				isOnlyOnceBeforeUseEnded = true;
			}

			// TODO : Supposedly an animation/sound effect or a wait period to show to player what is happening.
			await UniTask.Delay(waitDurationComplete.SecondsToMilliseconds(), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

			if (interactionType.HasFlag(InteractableType.AchieveRequirement_AfterEvent))
			{
				await SetDialogue(achievedRequiredAfterUseDialogue, initiatorStateMachine.transform);
			}

			await SetDialogue(rewardDialogue, initiatorStateMachine.transform);
			StageManager.Instance.GetReward(rewardInteractableObjectList).Forget();

			isGottenReward = true;
			if (!string.IsNullOrEmpty(endedDialogue)) SetIsInteractable(false);
		}

		protected async UniTask SetDialogue(string dialogue, Transform actor, Action doLast = default)
		{
			if (!dialogueSystemTrigger) return;

			dialogueSystemTrigger.conversation = dialogue;
			dialogueSystemTrigger?.OnUse(actor);

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive, cancellationToken: this.GetCancellationTokenOnDestroy());

			doLast?.Invoke();
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

