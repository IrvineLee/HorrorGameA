using System.Collections;
using UnityEngine;

using PixelCrushers.DialogueSystem;
using Helper;

namespace Personal.Dialogue
{
	public class DialogueSkip : MonoBehaviour
	{
		[SerializeField] float delay = 0.05f;

		AbstractDialogueUI dialogueUI;
		TextAnimatorContinueButtonFastForward fastForward;

		bool isSkip;
		CoroutineRun skipCR = new();

		void Awake()
		{
			dialogueUI = GetComponentInChildren<AbstractDialogueUI>(true);
			fastForward = GetComponentInChildren<TextAnimatorContinueButtonFastForward>(true);
		}

		public void SkipToResponseMenu(bool isFlag)
		{
			if (isFlag && !skipCR.IsDone) return;

			isSkip = isFlag;
			if (!isSkip) return;

			fastForward.OnFastForward();
			skipCR = CoroutineHelper.WaitFor(delay, dialogueUI.OnContinue);
		}

		void OnConversationLine(Subtitle subtitle)
		{
			skipCR.StopCoroutine();
			if (!isSkip) return;

			// Seems like calling dialogueUI.OnContinue will call this function next, so wait till next frame.
			CoroutineHelper.WaitNextFrame(() =>
			{
				fastForward.OnFastForward();
				skipCR = CoroutineHelper.WaitFor(delay, dialogueUI.OnContinue);
			});
		}

		void OnConversationResponseMenu(Response[] responses)
		{
			isSkip = false;
		}

		void OnConversationEnd(Transform actor)
		{
			isSkip = false;
			skipCR.StopCoroutine();
		}
	}
}