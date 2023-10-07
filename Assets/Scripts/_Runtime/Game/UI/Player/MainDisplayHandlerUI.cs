using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Personal.Quest;

namespace Personal.UI
{
	public class MainDisplayHandlerUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI questTMP = null;

		Dictionary<QuestInfo, int> questInfoOrderDictionary = new();

		public void SetQuest(QuestInfo questInfo)
		{
			//questTMP.text = text;
		}
	}
}

