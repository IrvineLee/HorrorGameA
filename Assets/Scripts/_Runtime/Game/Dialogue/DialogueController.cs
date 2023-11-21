using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.InputProcessing;

namespace Personal.Dialogue
{
	public class DialogueController : ControlInputBase
	{
		[SerializeField] DialogueSetup dialogueSetup = null;
		[SerializeField] DialogueSkip dialogueSkip = null;
		[SerializeField] DialogueSystemController dialogueSystemController = null;

		public DialogueSetup DialogueSetup { get => dialogueSetup; }
		public DialogueSkip DialogueSkip { get => dialogueSkip; }
		public DialogueSystemController DialogueSystemController { get => dialogueSystemController; }

		protected override void ButtonNorth()
		{
			DialogueSkip.Begin(true);
		}

		protected override void ButtonNorth_Released()
		{
			DialogueSkip.Begin(false);
		}
	}
}