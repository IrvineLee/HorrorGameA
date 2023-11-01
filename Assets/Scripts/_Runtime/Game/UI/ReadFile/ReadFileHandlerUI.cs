using System.Collections.Generic;
using UnityEngine;

using Personal.Manager;
using static Personal.Data.FileData;

namespace Personal.UI.Option
{
	public class ReadFileHandlerUI : UIHandlerBase
	{
		[SerializeField] UISelectionListingPage uISelectionListingPage = null;

		public void Read(List<Page> pageList)
		{
			UIManager.Instance.ReadFileUI.OpenWindow();
			uISelectionListingPage.UpdateSpawnedTMPText(pageList);
		}
	}
}