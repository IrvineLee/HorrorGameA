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
		[SerializeField] OptionSavedData optionSavedData = new OptionSavedData();

		public OptionSavedData OptionSavedData { get => optionSavedData; }
	}
}
