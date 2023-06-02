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

		[SerializeField] Transform menuParent = null;
		[SerializeField] List<Tab> tabList = new();

		public Transform MenuParent { get => menuParent; }
		public IWindowHandler IWindowHandler { get => this; }
		public IDefaultHandler IDefaultHandler { get => this; }

		public event Action<bool> OnMenuOpened;

		Dictionary<MenuTab, Tab> tabDictionary = new Dictionary<MenuTab, Tab>();
		MenuTab currentMenuTab;

		public void Initialize()
		{
			// Initialize all the tabs and set onClick listener.
			foreach (var tab in tabList)
			{
				_ = tab.OptionMenuUI.Initialize();
				tab.SelectButton.onClick.AddListener(() =>
				{
					currentMenuTab = tab.OptionMenuUI.MenuTab;

					CloseAllMenuTabs();
					tab.OptionMenuUI.gameObject.SetActive(true);
				});

				tabDictionary.Add(tab.OptionMenuUI.MenuTab, tab);
			}
		}

		/// <summary>
		/// This opens the entire option panel containing all the tabs.
		/// </summary>
		void IWindowHandler.OpenWindow()
		{
			SetupMenu(true);
			currentMenuTab = MenuTab.Game;

			// Open requested menu tab.
			if (tabDictionary.TryGetValue(MenuTab.Game, out Tab tab))
			{
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
			menuParent.gameObject.SetActive(isFlag);
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