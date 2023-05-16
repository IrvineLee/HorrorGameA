using Personal.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.UI.Option
{
	public class OptionHandlerUI : MonoBehaviour
	{
		public enum MenuTab
		{
			Graphic = 0,
			Audio,
			Control,
			Language,
		}

		[SerializeField] Transform background = null;
		[SerializeField] List<OptionMenuUI> optionMenuUIList = null;

		Dictionary<MenuTab, OptionMenuUI> optionMenuUIDictionary = new Dictionary<MenuTab, OptionMenuUI>();

		public static event Action<bool> OnMenuOpened;

		public void Initialize()
		{
			foreach (var option in optionMenuUIList)
			{
				_ = option.Initialize();
				optionMenuUIDictionary.Add(option.MenuTab, option);
			}
		}

		public void OpenMenuTab(MenuTab menuTab)
		{
			background.gameObject.SetActive(true);
			OnMenuOpened?.Invoke(true);

			// Close all tabs.
			foreach (var option in optionMenuUIList)
			{
				option.gameObject.SetActive(false);
			}

			// Open requested menu tab.
			if (optionMenuUIDictionary.TryGetValue(menuTab, out OptionMenuUI optionMenuUI))
			{
				optionMenuUI.gameObject.SetActive(true);
			}
		}

		public void CloseMenuTab()
		{
			background.gameObject.SetActive(false);
			OnMenuOpened?.Invoke(false);
		}
	}
}