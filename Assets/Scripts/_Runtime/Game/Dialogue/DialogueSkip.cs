using UnityEngine;

using PixelCrushers.DialogueSystem;
using Helper;

namespace Personal.Dialogue
{
	public class DialogueSkip : MonoBehaviour
	{
		[SerializeField] float initialWaitDuration = 0.5f;
		[SerializeField] float delay = 0.05f;

		AbstractDialogueUI dialogueUI;
		TextAnimatorContinueButtonFastForward fastForward;

		bool isSkip;

		CoroutineRun initialWaitCR = new();
		CoroutineRun skipCR = new();

		void Awake()
		{
			dialogueUI = GetComponentInChildren<AbstractDialogueUI>(true);
			fastForward = GetComponentInChildren<TextAnimatorContinueButtonFastForward>(true);
		}

		public void Begin(bool isFlag)
		{
			// Reset back to default if button is released.
			if (!isFlag)
			{
				StopSkip();
				return;
			}

			SkipDialogue();
			initialWaitCR = CoroutineHelper.WaitFor(initialWaitDuration, SkipDialogue);
		}

		void SkipDialogue()
		{
			isSkip = true;

			fastForward.OnFastForward();
			skipCR = CoroutineHelper.WaitFor(delay, dialogueUI.OnContinue);
		}

		void StopSkip()
		{
			isSkip = false;

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