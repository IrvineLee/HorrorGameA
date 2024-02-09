using System;

using Personal.Entity;
using Personal.UI.Window;

[Serializable]
public class WindowUIEntity : GenericNameEntity
{
	public WindowDisplayType windowDisplayType;
	public WindowButtonDisplayType buttonDisplayType;
	public string buttonAText;
	public string buttonBText;
	public string buttonCText;
	public float widthRatio;
	public float heightRatio;
	public string title_EN;
	public string description_EN;
}
