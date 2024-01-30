using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Personal.Data;
using Personal.Localization;
using Personal.Manager;

namespace Personal.UI
{
	public class UISelectionListingPage : UISelectionListing
	{
		[SerializeField] Button endButton = null;

		void OnEnable()
		{
			EnableEndButton(false);
		}

		public override void InitialSetup()
		{
			base.InitialSetup();
			endButton.onClick.AddListener(() => UIManager.Instance.CloseWindowStack());
		}

		/// <summary>
		/// This will use the spawned TMP to display the string. If not enough TMP, it will spawn it in.
		/// Typically use this if you don't want to update the stringList(pre-determined string).
		/// Ex: File read etc...
		/// </summary>
		public void UpdateSpawnedTMPText(FileData fileData)
		{
			var entity = MasterDataManager.Instance.ReadFile.Get((int)fileData.ReadFileType);
			List<string> strList = new();

			AddToList(entity.page01, strList);
			AddToList(entity.page02, strList);
			AddToList(entity.page03, strList);
			AddToList(entity.page04, strList);
			AddToList(entity.page05, strList);
			AddToList(entity.page06, strList);
			AddToList(entity.page07, strList);
			AddToList(entity.page08, strList);
			AddToList(entity.page09, strList);
			AddToList(entity.page10, strList);

			SpawnRequiredSelection(strList);
			HandleButtonVisibility();

			// Update the alignment.
			for (int i = 0; i < activeTMPList.Count; i++)
			{
				var tmp = activeTMPList[i];
				tmp.alignment = fileData.PageList[i].Alignment;
			}
		}

		protected override void HandleButtonVisibility()
		{
			leftButton.interactable = true;
			EnableEndButton(false);

			if (currentActiveIndex == 0) leftButton.interactable = false;
			else if (currentActiveIndex >= activeTMPList.Count - 1) EnableEndButton(true);
		}

		void AddToList(string s, List<string> strList)
		{
			if (!string.IsNullOrEmpty(s)) strList.Add(s);
		}

		void EnableEndButton(bool isFlag)
		{
			rightButton.gameObject.SetActive(!isFlag);
			endButton.gameObject.SetActive(isFlag);
		}

		void OnDisable()
		{
			selectionParentTrans.localPosition = Vector3.zero;
			currentActiveIndex = 0;
		}
	}
}
