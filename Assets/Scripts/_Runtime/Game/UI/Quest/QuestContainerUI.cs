using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TMPro;
using Personal.Quest;

namespace Personal.UI.Quest
{
	public class QuestContainerUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI questTitleTMP = null;
		[SerializeField] Transform descriptionTMPParent = null;

		public bool IsMainQuest { get; private set; }

		List<TextMeshProUGUI> descriptionTMPList = new();

		public void ShowQuest(QuestInfo questInfo)
		{
			if (descriptionTMPList.Count <= 0)
			{
				descriptionTMPList = descriptionTMPParent.GetComponentsInChildren<TextMeshProUGUI>().ToList();
			}

			IsMainQuest = questInfo.QuestEntity.isMainQuest;
			questTitleTMP.text = questInfo.QuestEntity.name;

			for (int i = 0; i < descriptionTMPList.Count; i++)
			{
				descriptionTMPList[i].text = questInfo.TaskInfoList[i].Description;
			}
		}
	}
}