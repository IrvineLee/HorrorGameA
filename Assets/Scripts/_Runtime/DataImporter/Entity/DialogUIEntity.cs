using System;

using Personal.Entity;
using static MasterDialogUI;
using static Personal.UI.Dialog.DialogBoxHandlerUI;

[Serializable]
public class DialogUIEntity : GenericEntity
{
	public DialogueUIType dialogueUIType;
	public string title_EN;
	public string description_EN;
	public DialogDisplayType dialogDisplayType;
	public ButtonDisplayType buttonDisplayType;
}
