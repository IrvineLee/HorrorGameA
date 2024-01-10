using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Personal.Item;
using Personal.InteractiveObject;
using PixelCrushers.DialogueSystem;
using Cysharp.Threading.Tasks;
using Helper;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "InteractableDefinition", menuName = "ScriptableObjects/Definition/Interactable", order = 0)]
	[Serializable]
	public class InteractableDefinition : ScriptableObject
	{
		#region Constants
		const string EXAMINABLE_BEFORE_KEY_EVENT_STRING = "@interactableType.HasFlag(InteractableType.ExaminableBeforeKeyEvent)";

		const string REQUIRED_STRING = "@interactableType.HasFlag(InteractableType.Requirement)";
		const string ACHIEVED_REQUIRED_BEFORE_USE_STRING = "@interactableType.HasFlag(InteractableType.Requirement) && interactableType.HasFlag(InteractableType.AchieveRequirement_BeforeInteract)";
		const string ACHIEVED_REQUIRED_AFTER_USE_STRING = "@interactableType.HasFlag(InteractableType.Requirement) && interactableType.HasFlag(InteractableType.AchieveRequirement_AfterInteract)";

		const string REWARD_STRING = "@interactableType.HasFlag(InteractableType.Reward)";
		const string END_STRING = "@interactableType.HasFlag(InteractableType.EndRemainInteractable)";
		#endregion

		[SerializeField] InteractableType interactableType;
		[SerializeField] InteractableCompleteType interactionCompleteType = InteractableCompleteType.NotInteractable;

		[SerializeField] bool isAllSameDatabase = false;
		[ShowIf("@isAllSameDatabase")] [SerializeField] DialogueDatabase dialogueDatabase = null;

		#region Examinable

		[ShowIf(EXAMINABLE_BEFORE_KEY_EVENT_STRING)]
		[Header("Examinable")]
		[Tooltip("The dialogue when the key event has not started and make this object truly interactable.")]
		[SerializeField] InteractDialogue examinableDialogue = null;

		[Tooltip("Key event to disable the examinable state.")]
		[SerializeField] KeyEventType keyEventEndType = KeyEventType._200000_None;

		#endregion

		#region Required Item

		[ShowIf(REQUIRED_STRING)]
		[Header("Requirement")]
		[Tooltip("The dialogue when the player does not have the required items to enable interaction.")]
		[SerializeField] InteractDialogue requiredItemDialogue = null;

		[ShowIf(REQUIRED_STRING)]
		[Tooltip("The item needed to enable/trigger the interaction." +
			" Some object might be interactable(pre-talk) but cannot be used/triggered/picked up until you get other items first.")]
		[SerializeField] List<ItemType> requiredItemTypeList = new();

		#endregion

		#region Achieved Required Before Interact

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Header("Achieved requirement, before interact")]
		[Tooltip("Does it happen only once?")]
		[SerializeField] bool isOnlyOnce_BeforeUse = false;

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Tooltip("The dialogue when you have the required item, but before using them.")]
		[SerializeField] InteractDialogue achievedRequiredBeforeDialogue = null;

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Tooltip("After dialogue, instantiate animator if have any. Typically used for starting the device etc. Ex: A keycard insert into the slot.")]
		[SerializeField] Animator beforeInteractPrefab = null;

		#endregion

		#region Achieved Required After Interact

		[ShowIf(ACHIEVED_REQUIRED_AFTER_USE_STRING)]
		[Header("Achieved requirement, after interact")]
		[Tooltip("The dialogue when you have the required item and after using them.")]
		[SerializeField] InteractDialogue achievedRequiredAfterDialogue = null;

		[ShowIf(ACHIEVED_REQUIRED_AFTER_USE_STRING)]
		[Tooltip("After dialogue, instantiate animator if have any. Typically used for end animation. Ex: Lighting up the bulb after completing it.")]
		[SerializeField] Animator afterInteractPrefab = null;

		#endregion

		#region Reward

		[ShowIf(REWARD_STRING)]
		[Header("Reward")]
		[Tooltip("The interaction dialogue when you get the reward.")]
		[SerializeField] InteractDialogue rewardDialogue = null;

		[ShowIf(REWARD_STRING)]
		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		#endregion

		#region End

		[ShowIf(END_STRING)]
		[Header("End Remain Interactable")]
		[Tooltip("The next interaction dialogue after getting the reward.")]
		[SerializeField] InteractDialogue endedDialogue = null;

		#endregion

		public InteractableType InteractionType { get => interactableType; }
		public InteractableCompleteType InteractionCompleteType { get => interactionCompleteType; }

		public string ExaminableDialogue { get => examinableDialogue.conversation; }
		public KeyEventType KeyEventType { get => keyEventEndType; }

		public string RequiredItemDialogue { get => requiredItemDialogue.conversation; }
		public List<ItemType> RequiredItemTypeList { get => requiredItemTypeList; }

		public bool IsOnlyOnce_BeforeUse { get => isOnlyOnce_BeforeUse; }
		public string AchievedRequiredBeforeUseDialogue { get => achievedRequiredBeforeDialogue.conversation; }
		public Animator BeforeInteractPrefab { get => beforeInteractPrefab; }

		public string AchievedRequiredAfterUseDialogue { get => achievedRequiredAfterDialogue.conversation; }
		public Animator AfterInteractPrefab { get => afterInteractPrefab; }

		public string RewardDialogue { get => rewardDialogue.conversation; }
		public List<InteractableObject> RewardInteractableObjectList { get => rewardInteractableObjectList; }

		public string EndedDialogue { get => endedDialogue.conversation; }

		public async UniTask SpawnAndPlayAnimator(Animator prefab, Transform parent, bool isEndDestroy = true)
		{
			if (!prefab) return;

			var instance = Instantiate(prefab, parent);

			string clipName = instance.GetCurrentAnimatorClipInfo(0)[0].clip.name;
			instance.Play(clipName);

			bool isDone = false;

			CoroutineHelper.WaitUntilCurrentAnimationEnds(instance, () => isDone = true);
			await UniTask.WaitUntil(() => isDone, cancellationToken: instance.GetCancellationTokenOnDestroy());

			if (!isEndDestroy) return;
			Destroy(instance.gameObject);
		}

		void OnValidate()
		{
			if (!isAllSameDatabase)
			{
				dialogueDatabase = null;
				return;
			}

			UpdateAllDialogueDatabase();
		}

		void UpdateAllDialogueDatabase()
		{
			examinableDialogue.dialogueDatabase = dialogueDatabase;
			requiredItemDialogue.dialogueDatabase = dialogueDatabase;
			achievedRequiredBeforeDialogue.dialogueDatabase = dialogueDatabase;
			achievedRequiredAfterDialogue.dialogueDatabase = dialogueDatabase;
			rewardDialogue.dialogueDatabase = dialogueDatabase;
			endedDialogue.dialogueDatabase = dialogueDatabase;
		}
	}
}