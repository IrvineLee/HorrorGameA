﻿using UnityEngine;

using Personal.Quest;
using Personal.UI.Quest;

namespace Personal.UI
{
	public class MainDisplayHandlerUI : MonoBehaviour
	{
		[SerializeField] QuestHandlerUI questHandler = null;

		public void SetQuest(QuestInfo questInfo)
		{
			questHandler.SetQuest(questInfo);
		}
	}
}

