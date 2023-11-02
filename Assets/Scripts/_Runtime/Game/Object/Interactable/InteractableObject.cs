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

namespace Personal.InteractiveObject
{
	public abstract class InteractableObject : GameInitialize
	{
		const string REQUIRED_STRING = "@interactionType == Personal.InteractiveObject.InteractableType.Requirement || " +
			"interactionType == Personal.InteractiveObject.InteractableType.Requirement_Reward";

		const string REWARD_STRING = "@interactionType == Personal.InteractiveObject.InteractableType.Reward || " +
			"interactionType == Personal.InteractiveObject.InteractableType.Requirement_Reward";

		[SerializeField] protected Transform colliderTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		[SerializeField] bool isInteractable = true;

		[SerializeField] InteractableType interactionType = InteractableType.Normal;

		[ShowIf(REQUIRED_STRING)]
		[Header("Requirement")]
		[Tooltip("The dialogue when the player does not have the required items to enable interaction.")]
		[SerializeField] DialogueSystemTrigger requiredItemTypeDialogue = null;

		[ShowIf(REQUIRED_STRING)]
		[Tooltip("The item needed to enable/trigger the interaction." +
			" Some object might be interactable(pre-talk) but cannot be used/triggered/picked up until you get other items first.")]
		[SerializeField] List<ItemType> requiredItemTypeList = new();

		[Header("Reward")]
		[ShowIf(REWARD_STRING)]
		[Tooltip("The interaction dialogue when you get the reward.")]
		[SerializeField] DialogueSystemTrigger rewardDialogue = null;

		[ShowIf(REWARD_STRING)]
		[Tooltip("The next interaction dialogue after getting the reward. Null means it's not interactable anymore")]
		[SerializeField] DialogueSystemTrigger afterRewardDialogue = null;

		[ShowIf(REWARD_STRING)]
		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		public Transform ColliderTrans { get => colliderTrans; }
		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine InitiatorStateMachine { get; protected set; }
		public bool IsInteractable { get => isInteractable; }

		protected MeshRenderer meshRenderer;
		protected OutlinableFadeInOut outlinableFadeInOut;

		protected bool isGottenReward;

		protected override void Initialize()
		{
			meshRenderer = GetComponentInChildren<MeshRenderer>(true);
			outlinableFadeInOut = colliderTrans.GetComponentInChildren<OutlinableFadeInOut>(true);

			SetIsInteractable(isInteractable);
		}

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			if (!isInteractable) return;

			if (!HasRequiredItems())
			{
				requiredItemTypeDialogue?.OnUse(initiatorStateMachine.transform);

				await UniTask.NextFrame();
				await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive, cancellationToken: this.GetCancellationTokenOnDestroy());

				return;
			}
			else if (isGottenReward)
			{
				afterRewardDialogue?.OnUse(initiatorStateMachine.transform);
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

		protected virtual async UniTask HandleInteraction() { await UniTask.CompletedTask; }

		protected virtual bool HasRequiredItems()
		{
			// Return if is Requirement/Requirement_Reward.
			if (interactionType != InteractableType.Requirement && interactionType != InteractableType.Requirement_Reward) return true;

			foreach (var item in requiredItemTypeList)
			{
				if (StageManager.Instance.PlayerController.Inventory.GetItemCount(item) <= 0) return false;
			}

			return true;
		}

		protected virtual async UniTask HandleGetReward(ActorStateMachine initiatorStateMachine)
		{
			// Return if is Reward/Requirement_Reward.
			if (interactionType != InteractableType.Reward && interactionType != InteractableType.Requirement_Reward) return;

			rewardDialogue?.OnUse(initiatorStateMachine.transform);

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive, cancellationToken: this.GetCancellationTokenOnDestroy());

			StageManager.Instance.GetReward(rewardInteractableObjectList).Forget();

			isGottenReward = true;
			if (!afterRewardDialogue) SetIsInteractable(false);
		}
	}
}

