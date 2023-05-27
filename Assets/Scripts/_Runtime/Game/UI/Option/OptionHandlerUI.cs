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

		[SerializeField] Transform menuParent = null;
		[SerializeField] Transform loadingIcon = null;
		[SerializeField] List<OptionMenuUI> optionMenuUIList = null;

		Dictionary<MenuTab, OptionMenuUI> optionMenuUIDictionary = new Dictionary<MenuTab, OptionMenuUI>();

		public Transform MenuParent { get => menuParent; }

		public static event Action<bool> OnMenuOpened;

		MenuTab currentMenuTab;

		public void Initialize()
		{
			foreach (var option in optionMenuUIList)
			{
				_ = option.Initialize();
				optionMenuUIDictionary.Add(option.MenuTab, option);
			}
		}

		public void SetCurrentMenuTab(MenuTab menuTab) { currentMenuTab = menuTab; }

		public void OpenOptionWindow(MenuTab menuTab = MenuTab.Game)
		{
			SetupMenu(true);
			currentMenuTab = menuTab;

			// Open requested menu tab.
			if (optionMenuUIDictionary.TryGetValue(menuTab, out OptionMenuUI optionMenuUI))
			{
				optionMenuUI.gameObject.SetActive(true);
			}
		}

		public void CloseOptionWindow()
		{
			foreach (var option in optionMenuUIList)
			{
				option.Save_Inspector();
			}

			SaveManager.Instance.SaveProfileData();
			InputManager.Instance.SetToDefaultActionMap();

			SetupMenu(false);
			loadingIcon.gameObject.SetActive(true);
		}

		public void CloseAllMenuTabs()
		{
			// Close all menu.
			foreach (var option in optionMenuUIList)
			{
				option.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Reset currently active option tab to default values.
		/// </summary>
		public void ResetToDefault()
		{
			if (optionMenuUIDictionary.TryGetValue(currentMenuTab, out OptionMenuUI optionMenuUI))
			{
				optionMenuUI.Default_Inspector();
			}
		}

		/// <summary>
		/// Setup the menu to be opened/closed.
		/// </summary>
		/// <param name="isFlag"></param>
		void SetupMenu(bool isFlag)
		{
			menuParent.gameObject.SetActive(isFlag);
			OnMenuOpened?.Invoke(isFlag);
			UIManager.Instance.FooterIconDisplay.gameObject.SetActive(isFlag);

			CloseAllMenuTabs();
		}
	}
}