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
			Game = 0,
			Graphic,
			Audio,
			Control,
		}

		[SerializeField] Transform menus = null;
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

		public void OpenOptionWindow(MenuTab menuTab = MenuTab.Game)
		{
			SetupMenu(true);

			// Open requested menu tab.
			if (optionMenuUIDictionary.TryGetValue(menuTab, out OptionMenuUI optionMenuUI))
			{
				optionMenuUI.gameObject.SetActive(true);
			}
		}

		public void CloseOptionWindow()
		{
			InputManager.Instance.SetToDefaultActionMap();
			SetupMenu(false);
		}

		public void CloseAllMenu()
		{
			// Close all menu.
			foreach (var option in optionMenuUIList)
			{
				option.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Setup the menu to be opened/closed.
		/// </summary>
		/// <param name="isFlag"></param>
		void SetupMenu(bool isFlag)
		{
			menus.gameObject.SetActive(isFlag);
			OnMenuOpened?.Invoke(isFlag);
			UIManager.Instance.FooterIconDisplay.gameObject.SetActive(isFlag);

			CloseAllMenu();
		}
	}
}