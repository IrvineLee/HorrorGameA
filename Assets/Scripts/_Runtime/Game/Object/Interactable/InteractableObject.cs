using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
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
		[SerializeField] Transform parentTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		[SerializeField] bool isInteractable = true;

		[Header("Requirement")]
		[Tooltip("The dialogue when the player does not have the required items to enable interaction.")]
		[SerializeField] DialogueSystemTrigger requiredItemTypeDialogue = null;

		[Tooltip("The item needed to enable/trigger the interaction." +
			" Some object might be interactable(pre-talk) but cannot be used/triggered/picked up until you get other items first.")]
		[SerializeField] List<ItemType> requiredItemTypeList = new();

		[Header("Reward")]
		[Tooltip("The next interaction dialogue after getting the reward. Null means it's not interactable anymore")]
		[SerializeField] DialogueSystemTrigger afterRewardDialogue = null;

		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		public Transform ParentTrans { get => parentTrans; }
		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine InitiatorStateMachine { get; protected set; }
		public bool IsInteractable { get => isInteractable; }

		protected Collider currentCollider;
		protected MeshRenderer meshRenderer;

		protected OutlinableFadeInOut outlinableFadeInOut;

		protected bool isGottenReward;

		protected override void Initialize()
		{
			currentCollider = GetComponentInChildren<Collider>(true);
			meshRenderer = GetComponentInChildren<MeshRenderer>(true);

			outlinableFadeInOut = GetComponentInChildren<OutlinableFadeInOut>(true);

			SetIsInteractable(isInteractable);
		}

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			if (!isInteractable) return;

			if (!HasRequiredItems())
			{
				requiredItemTypeDialogue?.OnUse(initiatorStateMachine.transform);

				await UniTask.NextFrame();
				await UniTask.WaitUntil(() => DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);

				return;
			}
			else if (isGottenReward)
			{
				afterRewardDialogue?.OnUse(initiatorStateMachine.transform);
				return;
			}

			InitiatorStateMachine = initiatorStateMachine;

			await HandleInteraction();
			if (rewardInteractableObjectList.Count > 0) GetReward();

			doLast?.Invoke();
		}

		public void ShowOutline(bool isFlag)
		{
			outlinableFadeInOut?.StartFade(isFlag);
		}

		public void SetIsInteractable(bool isFlag)
		{
			isInteractable = isFlag;
			if (currentCollider) currentCollider.enabled = isFlag;
		}

		protected virtual async UniTask HandleInteraction() { await UniTask.CompletedTask; }

		protected virtual bool HasRequiredItems()
		{
			foreach (var item in requiredItemTypeList)
			{
				if (StageManager.Instance.PlayerController.Inventory.GetItemCount(item) <= 0) return false;
			}

			return true;
		}

		protected virtual void GetReward()
		{
			StageManager.Instance.GetReward(rewardInteractableObjectList).Forget();

			isGottenReward = true;
			if (!afterRewardDialogue) SetIsInteractable(false);
		}
	}
}

