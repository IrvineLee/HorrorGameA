using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.UI.Option;
using Personal.Localization;
using Cysharp.Threading.Tasks;

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

		protected override void Initialize()
		{
			OptionGameUI.OnLanguageChangedEvent += OnLanguageChanged;
		}

		void OnLanguageChanged(SupportedLanguageType supportedLanguageType)
		{
			dialogueSystemController.SetLanguage(LanguageShorthand.Get(supportedLanguageType.ToString()));
		}

		public async UniTask WaitDialogueEnd()
		{
			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive, cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		void OnDestroy()
		{
			OptionGameUI.OnLanguageChangedEvent -= OnLanguageChanged;
		}
	}
}