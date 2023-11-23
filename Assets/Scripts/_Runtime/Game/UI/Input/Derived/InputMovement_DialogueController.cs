using UnityEngine;

using Personal.Manager;

namespace Personal.InputProcessing
{
	public class InputMovement_DialogueController : BasicControllerUI, IUIControlInput
	{
		void IUIControlInput.FastForward()
		{
			StageManager.Instance.DialogueController.DialogueSkip.Begin(true);
		}

		void IUIControlInput.FastForwardReleased()
		{
			StageManager.Instance.DialogueController.DialogueSkip.Begin(false);
		}
	}
}