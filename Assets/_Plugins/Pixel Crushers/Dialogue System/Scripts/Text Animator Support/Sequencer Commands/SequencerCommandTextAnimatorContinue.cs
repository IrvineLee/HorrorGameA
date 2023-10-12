using System.Collections;
using UnityEngine;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Syntax: TextAnimatorContinue([all])
    /// 
    /// - If 'all' is specified, continues all active conversations.
    /// Otherwise continues conversation associated with this sequencer.
    /// </summary>
    public class SequencerCommandTextAnimatorContinue : SequencerCommand
    {

        private const float Timeout = 2f; // Max time to wait for Show animation.

        private IEnumerator Start()
        {
            // Finish typewriter text:
            if (DialogueManager.isConversationActive && DialogueManager.standardDialogueUI != null)
            {
                DialogueActor dialogueActor;
                var panel = DialogueManager.standardDialogueUI.conversationUIElements.standardSubtitleControls.GetPanel(DialogueManager.currentConversationState.subtitle, out dialogueActor);
                if (panel != null)
                {
                    var typewriter = panel.subtitleText.gameObject.GetComponent<Febucci.UI.Core.TypewriterCore>();
                    if (typewriter != null)
                    {
                        if (panel.delayTypewriterUntilOpen && !panel.hasFocus)
                        {
                            float timeout = DialogueTime.time + Timeout;
                            while (!typewriter.isShowingText && DialogueTime.time < timeout)
                            {
                                yield return null;
                            }
                        }
                        yield return null;
                        yield return new WaitForEndOfFrame();
                        typewriter.SkipTypewriter();
                    }
                }
            }

            // Continue:
            var all = sequencer.conversationView == null || string.Equals("all", GetParameter(0), System.StringComparison.OrdinalIgnoreCase);
            if (all)
            {
                if (DialogueDebug.logInfo) Debug.Log("Dialogue System: Sequencer: TextAnimatorContinue(all)");
                DialogueManager.instance.BroadcastMessage(DialogueSystemMessages.OnConversationContinueAll, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (DialogueDebug.logInfo) Debug.Log("Dialogue System: Sequencer: TextAnimatorContinue()");
                sequencer.conversationView.HandleContinueButtonClick();
            }

            Stop();
        }
    }
}
