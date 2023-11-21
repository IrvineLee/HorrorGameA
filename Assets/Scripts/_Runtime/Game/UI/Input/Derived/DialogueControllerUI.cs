using UnityEngine;

using Personal.Manager;

namespace Personal.UI
{
	public class DialogueControllerUI : BasicControllerUI
	{
		protected override void ButtonNorth()
		{
			StageManager.Instance.DialogueController.DialogueSkip.Begin(true);
		}

		protected override void ButtonNorth_Released()
		{
			StageManager.Instance.DialogueController.DialogueSkip.Begin(false);
		}
	}
}