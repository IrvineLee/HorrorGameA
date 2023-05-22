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

		public bool IsOpened { get; private set; }

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
			SetupMenu(true);

			// Open requested menu tab.
			if (optionMenuUIDictionary.TryGetValue(menuTab, out OptionMenuUI optionMenuUI))
			{
				optionMenuUI.gameObject.SetActive(IsOpened);
			}
		}

		public void CloseMenuTab()
		{
			InputManager.Instance.SetToDefaultActionMap();
			SetupMenu(false);
		}

		/// <summary>
		/// Setup the menu to be opened/closed.
		/// </summary>
		/// <param name="isFlag"></param>
		void SetupMenu(bool isFlag)
		{
			IsOpened = isFlag;
			background.gameObject.SetActive(IsOpened);
			OnMenuOpened?.Invoke(IsOpened);

			// Close all tabs.
			foreach (var option in optionMenuUIList)
			{
				option.gameObject.SetActive(false);
			}
		}
	}
}