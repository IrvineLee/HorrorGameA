using System.Collections;
using UnityEngine;

using PixelCrushers.DialogueSystem;

namespace Personal.Dialogue
{
	public class DialogueSkip : MonoBehaviour
	{
		AbstractDialogueUI dialogueUI;
		bool isSkip;

		void Awake()
		{
			dialogueUI = GetComponentInChildren<AbstractDialogueUI>();
		}

		public void SkipToResponseMenu(bool isFlag)
		{
			isSkip = isFlag;
			dialogueUI.OnContinue();
		}

		void OnConversationLine(Subtitle subtitle)
		{
			if (isSkip) StartCoroutine(ContinueAtEndOfFrame());
		}

		IEnumerator ContinueAtEndOfFrame()
		{
			yield return new WaitForSeconds(0.05f);
			dialogueUI.OnContinue();
		}

		void OnConversationResponseMenu(Response[] responses)
		{
			isSkip = false;
		}

		void OnConversationEnd(Transform actor)
		{
			isSkip = false;
		}
	}
}