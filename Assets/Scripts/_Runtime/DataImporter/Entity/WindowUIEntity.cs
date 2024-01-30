using System;

using Personal.Entity;
using static Personal.UI.Window.WindowEnum;

[Serializable]
public class WindowUIEntity : GenericNameEntity
{
	public WindowDisplayType windowDisplayType;
	public ButtonDisplayType buttonDisplayType;
	public string buttonAText;
	public string buttonBText;
	public string buttonCText;
	public float widthRatio;
	public float heightRatio;
	public string title_EN;
	public string description_EN;
}
