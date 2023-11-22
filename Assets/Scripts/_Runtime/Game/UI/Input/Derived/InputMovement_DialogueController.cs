using UnityEngine;

using Personal.Manager;

namespace Personal.InputProcessing
{
	public class InputMovement_DialogueController : BasicControllerUI, IControlInput
	{
		void IControlInput.ButtonNorth()
		{
			StageManager.Instance.DialogueController.DialogueSkip.Begin(true);
		}

		void IControlInput.ButtonNorth_Released()
		{
			StageManager.Instance.DialogueController.DialogueSkip.Begin(false);
		}
	}
}