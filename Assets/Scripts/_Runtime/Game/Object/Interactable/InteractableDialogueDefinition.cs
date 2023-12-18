using System;
using System.Collections.Generic;
using UnityEngine;

using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using Personal.Item;
using Personal.InteractiveObject;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "InteractableDialogueDefinition", menuName = "ScriptableObjects/InteractableObject/Dialogue", order = 0)]
	[Serializable]
	public class InteractableDialogueDefinition : ScriptableObject
	{
		#region Constants
		const string EXAMINABLE_BEFORE_KEY_EVENT_STRING = "@interactionType.HasFlag(InteractableType.ExaminableBeforeKeyEvent)";

		const string REQUIRED_STRING = "@interactionType.HasFlag(InteractableType.Requirement)";
		const string ACHIEVED_REQUIRED_BEFORE_USE_STRING = "@interactionType.HasFlag(InteractableType.Requirement) && interactionType.HasFlag(InteractableType.AchieveRequirement_BeforeInteract)";
		const string ACHIEVED_REQUIRED_AFTER_USE_STRING = "@interactionType.HasFlag(InteractableType.Requirement) && interactionType.HasFlag(InteractableType.AchieveRequirement_AfterInteract)";

		const string REWARD_STRING = "@interactionType.HasFlag(InteractableType.Reward)";
		const string END_STRING = "@interactionType.HasFlag(InteractableType.End)";
		#endregion

		[SerializeField] InteractableType interactableType;
		[SerializeField] InteractableCompleteType interactableCompleteType = InteractableCompleteType.NotInteractable;

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
		[Header("Achieved requirement, before use")]
		[Tooltip("Does it happen only once?")]
		[SerializeField] bool isOnlyOnce_BeforeUse = false;

		[ShowIf(ACHIEVED_REQUIRED_BEFORE_USE_STRING)]
		[Tooltip("The dialogue when you have the required item, but before using them.")]
		[SerializeField] [ConversationPopup(true, true)] string achievedRequiredBeforeUseDialogue = null;

		#endregion

		#region Achieved Required After Use

		[ShowIf(ACHIEVED_REQUIRED_AFTER_USE_STRING)]
		[Header("Achieved requirement, after use")]
		[Tooltip("The dialogue when you have the required item and after using them.")]
		[SerializeField] [ConversationPopup(true, true)] string achievedRequiredAfterUseDialogue = null;

		#endregion

		#region Reward

		[ShowIf(REWARD_STRING)]
		[Header("Reward")]
		[Tooltip("The interaction dialogue when you get the reward.")]
		[SerializeField] [ConversationPopup(true, true)] string rewardDialogue = null;

		[ShowIf(REWARD_STRING)]
		[SerializeField] List<InteractableObject> rewardInteractableObjectList = new();

		#endregion

		#region End

		[ShowIf(END_STRING)]
		[Header("End")]
		[Tooltip("The next interaction dialogue after getting the reward.")]
		[SerializeField] [ConversationPopup(true, true)] string endedDialogue = null;

		#endregion


		public InteractableType InteractableType { get => interactableType; }
		public InteractableCompleteType InteractableCompleteType { get => interactableCompleteType; }

		public string ExaminableDialogue { get => examinableDialogue; }

		public string RequiredItemDialogue { get => requiredItemDialogue; }
		public List<ItemType> RequiredItemTypeList { get => requiredItemTypeList; }

		public bool IsOnlyOnce_BeforeUse { get => isOnlyOnce_BeforeUse; }
		public string AchievedRequiredBeforeUseDialogue { get => achievedRequiredBeforeUseDialogue; }

		public string AchievedRequiredAfterUseDialogue { get => achievedRequiredAfterUseDialogue; }

		public string RewardDialogue { get => rewardDialogue; }
		public List<InteractableObject> RewardInteractableObjectList { get => rewardInteractableObjectList; }

		public string EndedDialogue { get => endedDialogue; }
	}
}