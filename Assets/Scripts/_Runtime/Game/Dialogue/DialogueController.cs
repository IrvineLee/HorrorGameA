using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.GameState;

namespace Personal.Dialogue
{
	public class DialogueController : GameInitialize
	{
		[SerializeField] DialogueSetup dialogueSetup = null;
		[SerializeField] DialogueSkip dialogueSkip = null;
		[SerializeField] DialogueSystemController dialogueSystemController = null;

		public DialogueSetup DialogueSetup { get => dialogueSetup; }
		public DialogueSkip DialogueSkip { get => dialogueSkip; }
		public DialogueSystemController DialogueSystemController { get => dialogueSystemController; }
	}
}