using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Personal.Manager;
using static Personal.UI.Window.WindowEnum;

namespace Personal.UI.Option
{
	public class OptionHandlerUI : MonoBehaviour, IWindowHandler, IDefaultHandler
	{
		public enum MenuTab
		{
			Game = 0,
			Graphic,
			Audio,
			Control,
		}

		[Serializable]
		public class Tab
		{
			[SerializeField] Button selectButton = null;
			[SerializeField] OptionMenuUI optionMenuUI = null;

			public Button SelectButton { get => selectButton; }
			public OptionMenuUI OptionMenuUI { get => optionMenuUI; }
		}

		[SerializeField] List<Tab> tabList = new();

		public IWindowHandler IWindowHandler { get => this; }
		public IDefaultHandler IDefaultHandler { get => this; }

		public event Action<bool> OnMenuOpened;

		Dictionary<MenuTab, Tab> tabDictionary = new Dictionary<MenuTab, Tab>();

		MenuTab currentMenuTab;
		int currentMenuIndex;

		public void Initialize()
		{
			// Initialize all the tabs and set onClick listener.
			for (int i = 0; i < tabList.Count; i++)
			{
				Tab tab = tabList[i];
				int index = i;

				tab.OptionMenuUI.InitialSetup();
				tab.SelectButton.onClick.AddListener(() =>
				{
					currentMenuTab = tab.OptionMenuUI.MenuTab;
					currentMenuIndex = index;

					CloseAllMenuTabs();
					tab.OptionMenuUI.gameObject.SetActive(true);
				});

				tabDictionary.Add(tab.OptionMenuUI.MenuTab, tab);
			}

			// Set game tab to be in a pressed state.
			tabDictionary.TryGetValue(MenuTab.Game, out Tab gameTab);
			gameTab.SelectButton.interactable = false;
		}

		/// <summary>
		/// Enable all tab buttons.
		/// </summary>
		public void EnableAllTabButtons()
		{
			foreach (var tab in tabList)
			{
				tab.SelectButton.interactable = true;
			}
		}

		/// <summary>
		/// Open next/previous tab. 
		/// </summary>
		/// <param name="menuTab"></param>
		public void NextTab(bool isNextTab)
		{
			int index = isNextTab ? currentMenuIndex + 1 : currentMenuIndex - 1;
			if (index < 0 || index > tabList.Count - 1) return;

			tabList[index].SelectButton.onClick.Invoke();
			currentMenuIndex = index;
		}

		/// <summary>
		/// This opens the entire option panel containing all the tabs.
		/// </summary>
		void IWindowHandler.OpenWindow()
		{
			SetupMenu(true);
			currentMenuTab = MenuTab.Game;

			if (tabDictionary.TryGetValue(currentMenuTab, out Tab tab))
			{
				tab.SelectButton.interactable = false;
				tab.OptionMenuUI.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Close the entire option panel.
		/// </summary>
		void IWindowHandler.CloseWindow()
		{
			foreach (var tab in tabList)
			{
				tab.SelectButton.interactable = true;
				tab.OptionMenuUI.Save_Inspector();
			}

			SaveManager.Instance.SaveProfileData();
			InputManager.Instance.SetToDefaultActionMap();

			SetupMenu(false);
			UIManager.Instance.ToolsHandlerUI.LoadingIconTrans.gameObject.SetActive(true);
		}

		/// <summary>
		/// Reset currently active option tab to default values.
		/// </summary>
		void IDefaultHandler.ResetToDefault()
		{
			if (!tabDictionary.TryGetValue(currentMenuTab, out Tab tab)) return;

			Action yesAction = () => tab.OptionMenuUI.Default_Inspector();
			_ = UIManager.Instance.WindowHandlerUI.OpenWindow(WindowUIType.DefaultButton, yesAction);
		}

		/// <summary>
		/// Setup the menu to be opened/closed.
		/// </summary>
		/// <param name="isFlag"></param>
		void SetupMenu(bool isFlag)
		{
			gameObject.SetActive(isFlag);
			OnMenuOpened?.Invoke(isFlag);
			UIManager.Instance.FooterIconDisplay.gameObject.SetActive(isFlag);

			CloseAllMenuTabs();
		}

		void CloseAllMenuTabs()
		{
			foreach (var tab in tabList)
			{
				tab.OptionMenuUI.gameObject.SetActive(false);
			}
		}
	}
}