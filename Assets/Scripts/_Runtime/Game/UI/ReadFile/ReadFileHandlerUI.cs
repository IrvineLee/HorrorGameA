using System.Collections.Generic;
using UnityEngine;

using Personal.Manager;
using Personal.Data;

namespace Personal.UI
{
	public class ReadFileHandlerUI : UIHandlerBase
	{
		[SerializeField] UISelectionListingPage uISelectionListingPage = null;

		public void Read(FileData fileData)
		{
			UIManager.Instance.ReadFileUI.OpenWindow();
			uISelectionListingPage.UpdateSpawnedTMPText(fileData);
		}
	}
}