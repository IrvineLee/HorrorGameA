using UnityEngine;
using System;

namespace Personal.Save
{
	[Serializable]
	public class SlotSavedData
	{
		[SerializeField] string currentArea = "";
		[SerializeField] DateTime currentDataTime = DateTime.MinValue;
		[SerializeField] DateTime totalPlayTime = DateTime.MinValue;
		[SerializeField] int totalSave = 0;

		public string CurrentArea { get => currentArea; set => currentArea = value; }
		public DateTime CurrentDataTime { get => currentDataTime; set => currentDataTime = value; }
		public DateTime TotalPlayTime { get => totalPlayTime; set => totalPlayTime = value; }
		public int TotalSave { get => totalSave; set => totalSave = value; }
	}
}