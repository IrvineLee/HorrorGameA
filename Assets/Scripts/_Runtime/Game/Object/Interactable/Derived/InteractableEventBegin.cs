using UnityEngine;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.FSM;
using Personal.Manager;
using Personal.InputProcessing;
using Helper;

namespace Personal.InteractiveObject
{
	public class InteractableEventBegin : InteractableObject
	{
		#region Constants

		const string BEFORE_INTERACT_BEFORE_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.BeforeInteract_BeforeTalk)";
		const string BEFORE_INTERACT_AFTER_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.BeforeInteract_AfterTalk)";
		const string After_INTERACT_BEFORE_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.AfterInteract_BeforeTalk)";
		const string After_INTERACT_AFTER_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.AfterInteract_AfterTalk)";

		#endregion

		[SerializeField] ActionMapType actionMapType = ActionMapType.None;
		[SerializeField] InteractableAnimatorType interactableAnimatorType;

		[ShowIf(BEFORE_INTERACT_BEFORE_TALK_STRING)]
		[Tooltip("Before interaction, before dialogue, play animator if have any. Typically used for starting the device etc. Ex: A keycard insert into the slot.")]
		[SerializeField] Animator beforeInteractBeforeTalkAnimator = null;

		[ShowIf(BEFORE_INTERACT_AFTER_TALK_STRING)]
		[Tooltip("Before interaction, after dialogue, play animator if have any. Typically used for starting the device etc. Ex: A keycard insert into the slot.")]
		[SerializeField] Animator beforeInteractAfterTalkAnimator = null;

		[ShowIf(After_INTERACT_BEFORE_TALK_STRING)]
		[Tooltip("After interaction, before dialogue, play animator if have any. Typically used for end animation. Ex: Lighting up the bulb after completing it.")]
		[SerializeField] Animator afterInteractBeforeTalkAnimator = null;

		[ShowIf(After_INTERACT_AFTER_TALK_STRING)]
		[Tooltip("After interaction, after dialogue, play animator if have any. Typically used for end animation. Ex: Lighting up the bulb after completing it.")]
		[SerializeField] Animator afterInteractAfterTalkAnimator = null;

		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;

		protected override void Initialize()
		{
			base.Initialize();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
		}

		protected override async UniTask HandleInteraction()
		{
			// When events happened, hide the items.
			StageManager.Instance.PlayerController.Inventory.FPS_HideItem();

			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();
			ifsmHandler?.OnBegin(null);

			InputManager.Instance.EnableActionMap(actionMapType);
			await orderedStateMachine.Begin(interactionAssign, InitiatorStateMachine);

			InputManager.Instance.SetToDefaultActionMap();
			ifsmHandler?.OnExit();
		}

		protected override async UniTask HandleAnimator(InteractableAnimatorType interactableAnimatorType)
		{
			Animator animator = null;
			switch (interactableAnimatorType)
			{
				case InteractableAnimatorType.BeforeInteract_BeforeTalk:
					animator = beforeInteractBeforeTalkAnimator;
					break;
				case InteractableAnimatorType.BeforeInteract_AfterTalk:
					animator = beforeInteractAfterTalkAnimator;
					break;
				case InteractableAnimatorType.AfterInteract_BeforeTalk:
					animator = afterInteractBeforeTalkAnimator;
					break;
				case InteractableAnimatorType.AfterInteract_AfterTalk:
					animator = afterInteractAfterTalkAnimator;
					break;
			}

			if (!animator) return;
			await PlayAnimator(animator);
		}

		async UniTask PlayAnimator(Animator animator)
		{
			string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
			animator.Play(clipName);

			bool isDone = false;

			CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => isDone = true);
			await UniTask.WaitUntil(() => isDone, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
		}
	}
}

