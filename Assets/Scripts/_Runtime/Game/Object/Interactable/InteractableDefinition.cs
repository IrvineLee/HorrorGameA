using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;
using Personal.Item;
using Personal.KeyEvent;
using Personal.InteractiveObject;

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
		const string END_STRING = "@interactableType.HasFlag(InteractableType.EndDialogue)";

		const string BEFORE_INTERACT_BEFORE_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.BeforeInteract_BeforeTalk)";
		const string BEFORE_INTERACT_AFTER_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.BeforeInteract_AfterTalk)";
		const string After_INTERACT_BEFORE_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.AfterInteract_BeforeTalk)";
		const string After_INTERACT_AFTER_TALK_STRING = "@interactableAnimatorType.HasFlag(InteractableAnimatorType.AfterInteract_AfterTalk)";

		#endregion

		[Header("Key Event")]
		[Tooltip("The required key events for this object to be interactable.")]
		[SerializeField] List<KeyEventType> requiredKeyEventTypeList = new();

		[Tooltip("The key event upon completion of this object, after the reward dialogue(even if it's empty).")]
		[SerializeField] KeyEventType completeKeyEventType = KeyEventType.None;

		[Header("Interactable")]
		[SerializeField] InteractableType interactableType;
		[SerializeField] InteractableCompleteType interactableCompleteType = InteractableCompleteType.NotInteractable;

		[SerializeField] bool isAllSameDatabase = false;
		[ShowIf("@isAllSameDatabase")] [SerializeField] DialogueDatabase dialogueDatabase = null;

		#region Animation Parameter

		[Space]
		[SerializeField] InteractableAnimatorType interactableAnimatorType;

		[ShowIf(BEFORE_INTERACT_BEFORE_TALK_STRING)]
		[Tooltip("BEFORE interaction, BEFORE dialogue, play parameter in animator. Typically used for starting the device etc. Ex: A keycard insert into the slot.")]
		[ReadOnly] [SerializeField] string beforeInteractBeforeTalkParam = "BeforeInteract_BeforeTalk";

		[ShowIf(BEFORE_INTERACT_AFTER_TALK_STRING)]
		[Tooltip("BEFORE interaction, AFTER dialogue, play parameter in animator. Typically used for starting the device etc. Ex: A keycard insert into the slot.")]
		[ReadOnly] [SerializeField] string beforeInteractAfterTalkParam = "BeforeInteract_AfterTalk";

		[ShowIf(After_INTERACT_BEFORE_TALK_STRING)]
		[Tooltip("AFTER interaction, BEFORE dialogue, play parameter in animator. Typically used for end animation. Ex: Lighting up the bulb after completing it.")]
		[ReadOnly] [SerializeField] string afterInteractBeforeTalkParam = "AfterInteract_BeforeTalk";

		[ShowIf(After_INTERACT_AFTER_TALK_STRING)]
		[Tooltip("AFTER interaction, AFTER dialogue, play parameter in animator. Typically used for end animation. Ex: Lighting up the bulb after completing it.")]
		[ReadOnly] [SerializeField] string afterInteractAfterTalkParam = "AfterInteract_AfterTalk";

		#endregion

		#region Examinable

		[ShowIf(EXAMINABLE_BEFORE_KEY_EVENT_STRING)]
		[Header("1_Examinable (Before Key Event)")]
		[Tooltip("The dialogue when the key event has not started and make this object truly interactable.")]
		[SerializeField] InteractDialogue examinableDialogue = null;

		[ShowIf(EXAMINABLE_BEFORE_KEY_EVENT_STRING)]
		[Tooltip("Key event to disable the examinable state.")]
		[SerializeField] KeyEventType keyEventEndExaminableType = KeyEventType.None;

		#endregion

		#region Required Item

		[ShowIf(REQUIRED_STRING)]
		[Header("2_Requirement (Need all items at the same time)")]
		[Tooltip("The dialogue when the player does not have the required items to enable interaction.")]
		[SerializeField] InteractDialogue requiredItemDialogue = null;

		[ShowIf(REQUIRED_STRING)]
		[Tooltip("The item needed to enable/trigger the interaction." +
			" Some object might be interactable(pre-talk) but cannot be used/triggered/picked up until you get other items first.")]
		[SerializeField] List<ItemData> requiredItemList = new();

		#endregion

		#region Achieved Required Before Interact

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Header("3_Achieved requirement, before interact")]
		[Tooltip("Does it happen only once?")]
		[SerializeField] bool isOnlyOnce_BeforeUse = false;

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Tooltip("The dialogue when you have the required item, but before using them.")]
		[SerializeField] InteractDialogue achievedRequiredBeforeDialogue = null;

		#endregion

		#region Achieved Required After Interact

		[ShowIf(ACHIEVED_REQUIRED_AFTER_USE_STRING)]
		[Header("4_Achieved requirement, after interact")]
		[Tooltip("The dialogue when you have the required item and after using them.")]
		[SerializeField] InteractDialogue achievedRequiredAfterDialogue = null;

		#endregion

		#region Reward

		[ShowIf(REWARD_STRING)]
		[Header("5_Reward")]
		[Tooltip("The interaction dialogue when you get the reward.")]
		[SerializeField] InteractDialogue rewardDialogue = null;

		[ShowIf(REWARD_STRING)]
		[Tooltip("The reward you get after the dialogue.")]
		[SerializeField] List<ItemData> rewardItemList = new();

		#endregion

		#region End

		[ShowIf(END_STRING)]
		[Header("6_End Remain Interactable")]
		[Tooltip("The next interaction dialogue after getting the reward.")]
		[SerializeField] InteractDialogue endedDialogue = null;

		#endregion

		public List<KeyEventType> RequiredKeyEventTypeList { get => requiredKeyEventTypeList; }
		public KeyEventType CompleteKeyEventType { get => completeKeyEventType; }

		public InteractableType InteractionType { get => interactableType; }
		public InteractableCompleteType InteractableCompleteType { get => interactableCompleteType; }

		public string BeforeInteractBeforeTalkParam { get => beforeInteractBeforeTalkParam; }
		public string BeforeInteractAfterTalkParam { get => beforeInteractAfterTalkParam; }
		public string AfterInteractBeforeTalkParam { get => afterInteractBeforeTalkParam; }
		public string AfterInteractAfterTalkParam { get => afterInteractAfterTalkParam; }

		public string ExaminableDialogue { get => examinableDialogue.conversation; }
		public KeyEventType KeyEventEndExaminableType { get => keyEventEndExaminableType; }

		public string RequiredItemDialogue { get => requiredItemDialogue.conversation; }
		public List<ItemData> RequiredItemTypeList { get => requiredItemList; }

		public bool IsOnlyOnce_BeforeUse { get => isOnlyOnce_BeforeUse; }
		public string AchievedRequiredBeforeUseDialogue { get => achievedRequiredBeforeDialogue.conversation; }

		public string AchievedRequiredAfterUseDialogue { get => achievedRequiredAfterDialogue.conversation; }

		public string RewardDialogue { get => rewardDialogue.conversation; }
		public List<ItemData> RewardItemList { get => rewardItemList; }

		public string EndedDialogue { get => endedDialogue.conversation; }

		public bool HasFlag<T>(T enumType) where T : Enum
		{
			if (typeof(T) == typeof(InteractableType))
			{
				return interactableType.HasFlag(enumType);
			}
			else if (typeof(T) == typeof(InteractableCompleteType))
			{
				return interactableCompleteType.HasFlag(enumType);
			}
			else if (typeof(T) == typeof(InteractableAnimatorType))
			{
				return interactableAnimatorType.HasFlag(enumType);
			}

			return false;
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