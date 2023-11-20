using UnityEngine;

using PixelCrushers.DialogueSystem;
using Helper;
using Personal.Manager;
using Personal.GameState;

namespace Personal.Dialogue
{
	public class DialogueSkip : GameInitialize
	{
		[SerializeField] float initialWaitDuration = 0.5f;
		[SerializeField] float delay = 0.05f;

		AbstractDialogueUI dialogueUI;
		TextAnimatorContinueButtonFastForward fastForward;
		DialogueSetup dialogueSetup;

		bool isSkip;

		CoroutineRun initialWaitCR = new();
		CoroutineRun skipCR = new();

		protected override void Initialize()
		{
			dialogueUI = GetComponentInChildren<AbstractDialogueUI>(true);
			fastForward = GetComponentInChildren<TextAnimatorContinueButtonFastForward>(true);
			dialogueSetup = StageManager.Instance.DialogueController.DialogueSetup;
		}

		public void Begin(bool isFlag)
		{
			// Reset back to default if button is released.
			if (!isFlag)
			{
				StopSkip();
				dialogueSetup.SubtitleSetting.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;

				return;
			}

			SkipDialogue();
			initialWaitCR = CoroutineHelper.WaitFor(initialWaitDuration, SkipDialogue);
		}

		void SkipDialogue()
		{
			isSkip = true;
			dialogueSetup.SubtitleSetting.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Never;

			fastForward.OnFastForward();
			skipCR = CoroutineHelper.WaitFor(delay, dialogueUI.OnContinue);
		}

		void StopSkip()
		{
			isSkip = false;
			dialogueSetup.ContinueButton.gameObject.SetActive(!dialogueSetup.IsWaitingResponse);

			initialWaitCR.StopCoroutine();
			skipCR.StopCoroutine();

		}

		void OnConversationLine(Subtitle subtitle)
		{
			skipCR.StopCoroutine();
			if (!isSkip) return;

			// Seems like calling dialogueUI.OnContinue will call this function next, so wait till next frame.
			skipCR = CoroutineHelper.WaitNextFrame(() =>
			{
				fastForward.OnFastForward();
				skipCR = CoroutineHelper.WaitFor(delay, dialogueUI.OnContinue);
			});
		}

		void OnConversationResponseMenu(Response[] responses)
		{
			StopSkip();
		}

		void OnConversationEnd(Transform actor)
		{
			StopSkip();
		}
	}
}