using System;
using UnityEngine;

namespace Personal.Save
{
	/// <summary>
	/// This saves the general info of the user.
	/// </summary>
	[Serializable]
	public class SaveProfile : GenericSave
	{
		[SerializeField] SettingSavedData settingSavedData = new SettingSavedData();

		public SettingSavedData SettingSavedData { get => settingSavedData; }
	}
}
