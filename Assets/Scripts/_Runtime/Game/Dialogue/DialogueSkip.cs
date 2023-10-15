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
		bool isSkip;

		CoroutineRun skipCR = new();

		void Awake()
		{
			dialogueUI = GetComponentInChildren<AbstractDialogueUI>();
		}

		public void SkipToResponseMenu(bool isFlag)
		{
			if (isFlag && !skipCR.IsDone) return;

			isSkip = isFlag;
			if (!isSkip) return;

			dialogueUI.OnContinue();
		}

		void OnConversationLine(Subtitle subtitle)
		{
			skipCR.StopCoroutine();
			if (isSkip) skipCR = CoroutineHelper.WaitFor(delay, dialogueUI.OnContinue);
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