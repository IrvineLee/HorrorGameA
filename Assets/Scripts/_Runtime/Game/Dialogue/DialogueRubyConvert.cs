using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.GameState;
using TMPro;
using Helper;

namespace Personal.Dialogue
{
	public class DialogueRubyConvert : GameInitialize
	{
		RubyTextMeshProUGUI rubyConversationTMP;

		protected override void Initialize()
		{
			rubyConversationTMP = GetComponentInChildren<RubyTextMeshProUGUI>(true);
		}

		void OnConversationLine(Subtitle subtitle)
		{
			CoroutineHelper.WaitEndOfFrame(() => rubyConversationTMP.uneditedText = subtitle.formattedText.text);
		}
	}
}