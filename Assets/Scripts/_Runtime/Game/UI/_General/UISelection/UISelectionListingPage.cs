using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static Personal.Data.FileData;

namespace Personal.UI
{
	public class UISelectionListingPage : UISelectionListing
	{
		/// <summary>
		/// This will use the spawned TMP to display the string. If not enough TMP, it will spawn it in.
		/// Typically use this if you don't want to update the stringList(pre-determined string).
		/// Ex: File read etc...
		/// </summary>
		public void UpdateSpawnedTMPText(List<Page> pageList)
		{
			List<string> strList = pageList.Select(x => x.Str).ToList();

			SpawnRequiredSelection(strList);
			HandleButtonVisibility();

			// Update the alignment.
			for (int i = 0; i < activeTMPList.Count; i++)
			{
				var tmp = activeTMPList[i];
				tmp.alignment = pageList[i].Alignment;
			}
		}

		void OnDisable()
		{
			selectionParentTrans.localPosition = Vector3.zero;
			currentActiveIndex = 0;
		}
	}
}
