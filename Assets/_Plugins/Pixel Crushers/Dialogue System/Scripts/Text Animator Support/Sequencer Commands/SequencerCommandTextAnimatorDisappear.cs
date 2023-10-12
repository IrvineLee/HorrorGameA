using Febucci.UI.Core;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Syntax: TextAnimatorDisappear()
    /// Hides current subtitle panel's subtitle text using Text Animator.
    /// </summary>
    public class SequencerCommandTextAnimatorDisappear : SequencerCommand
    {

        private TypewriterCore typewriter;

        private void Awake()
        {
            DialogueActor dialogueActor;
            var panel = DialogueManager.standardDialogueUI.conversationUIElements.standardSubtitleControls.GetPanel(DialogueManager.currentConversationState.subtitle, out dialogueActor);
            typewriter = panel.subtitleText.gameObject.GetComponent<TypewriterCore>();
            if (typewriter == null)
            {
                Stop();
            }
            else
            {
                typewriter.onTextDisappeared.AddListener(OnTextDisappeared);
                typewriter.StartDisappearingText();
            }
        }

        private void OnTextDisappeared()
        {
            typewriter.onTextDisappeared.RemoveListener(OnTextDisappeared);
            Stop();
        }

        private void OnDestroy()
        {
            if (typewriter != null)
            {
                typewriter.onTextDisappeared.RemoveListener(OnTextDisappeared);
            }
        }
    }
}
