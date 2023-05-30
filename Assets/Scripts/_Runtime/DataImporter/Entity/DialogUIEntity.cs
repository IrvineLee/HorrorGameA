using System;

using Personal.Entity;
using static Personal.UI.Dialog.DialogBoxEnum;

[Serializable]
public class DialogUIEntity : GenericEntity
{
	public DialogUIType dialogUIType;
	public DialogDisplayType dialogDisplayType;
	public ButtonDisplayType buttonDisplayType;
	public string buttonAText;
	public string buttonBText;
	public string buttonCText;
	public float widthRatio;
	public float heightRatio;
	public string title_EN;
	public string description_EN;
}
