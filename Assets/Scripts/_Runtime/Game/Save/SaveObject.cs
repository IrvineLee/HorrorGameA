using System;
using UnityEngine;

namespace Personal.Save
{
	[Serializable]
	public class SaveObject : GenericSave
	{
		[SerializeField] PlayerSavedData playerSavedData = new PlayerSavedData();

		public PlayerSavedData PlayerSavedData { get => playerSavedData; }
	}
}
