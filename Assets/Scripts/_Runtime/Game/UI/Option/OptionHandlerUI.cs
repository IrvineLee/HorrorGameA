using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Personal.Manager;
using static Personal.UI.Window.WindowEnum;
using Personal.InputProcessing;

namespace Personal.UI.Option
{
	public class OptionHandlerUI : UIHandlerBase, IDefaultHandler
	{
		public enum MenuTab
		{
			Game = 0,
			Graphic,
			Audio,
			Control,
		}

		[SerializeField] MenuTab startMenuTab = MenuTab.Game;
		[SerializeField] List<Tab> tabList = new();
		[SerializeField] [ReadOnly] List<Tab> bottomTabList = new();

		public IReadOnlyDictionary<MenuTab, Tab> TabDictionary { get => tabDictionary; }

		Dictionary<MenuTab, Tab> tabDictionary = new Dictionary<MenuTab, Tab>();

		MenuTab currentMenuTab;
		int currentMenuIndex;
		int currentBottomMenuIndex;

		public override void InitialSetup()
		{
			base.InitialSetup();
			IDefaultHandler = this;

			InitializeTabs();
			InitializeIgnoredSelection();
		}

		public override void OpenWindow()
		{
			base.OpenWindow();

			UIManager.Instance.FooterIconDisplay.Begin(true);

			DisableAllTabs();
			if (!tabDictionary.TryGetValue(startMenuTab, out Tab tab)) return;

			// Activate current menu tab.
			tab.OptionMenuUI.gameObject.SetActive(true);
		}

		public override void CloseWindow(bool isInstant)
		{
			base.CloseWindow(isInstant);

			currentMenuTab = startMenuTab;
			UIManager.Instance.FooterIconDisplay.Begin(false);

			foreach (var tab in tabList)
			{
				tab.OptionMenuUI.Save_Inspector();
			}

			if (!OptionMenuUI.IsChangesMade) return;
			SaveManager.Instance.SaveProfileData();
		}

		/// <summary>
		/// Update the active bottom tab, if applicable.
		/// </summary>
		/// <param name="bottomTabList"></param>
		public void UpdateBottomTab(List<Tab> bottomTabList)
		{
			currentBottomMenuIndex = 0;
			this.bottomTabList = null;

			if (bottomTabList.Count <= 0) return;
			this.bottomTabList = bottomTabList;
		}

		/// <summary>
		/// Open next/previous tab. 
		/// </summary>
		/// <param name="menuTab"></param>
		public void NextTopTab(bool isNextTab)
		{
			ToNextTab(isNextTab, tabList, ref currentMenuIndex);
		}

		/// <summary>
		/// Open next/previous bottom tab, if applicable. 
		/// </summary>
		/// <param name="isNextTab"></param>
		public void NextBottomTab(bool isNextTab)
		{
			if (bottomTabList == null || bottomTabList.Count <= 0) return;
			ToNextTab(isNextTab, bottomTabList, ref currentBottomMenuIndex);
		}

		/// <summary>
		/// Handle the switching of tabs.
		/// </summary>
		/// <param name="isNextTab"></param>
		/// <param name="selectedTabList"></param>
		/// <param name="currentMenuIndex"></param>
		void ToNextTab(bool isNextTab, List<Tab> selectedTabList, ref int currentMenuIndex)
		{
			int index = isNextTab ? currentMenuIndex + 1 : currentMenuIndex - 1;
			if (index < 0 || index > selectedTabList.Count - 1) return;

			selectedTabList[index].SelectButton.onClick.Invoke();
			currentMenuIndex = index;
		}

		/// <summary>
		/// Initialize the tabs.
		/// </summary>
		void InitializeTabs()
		{
			// Initialize all the tabs and set onClick listener.
			for (int i = 0; i < tabList.Count; i++)
			{
				Tab tab = tabList[i];
				int index = i;

				// Setup the tabs.
				tab.OptionMenuUI.InitialSetup();
				tab.SelectButton.onClick.AddListener(() =>
				{
					currentMenuTab = tab.OptionMenuUI.MenuTab;
					currentMenuIndex = index;

					DisableAllTabs();
					tab.OptionMenuUI.gameObject.SetActive(true);
				});

				tabDictionary.Add(tab.OptionMenuUI.MenuTab, tab);
			}
		}

		/// <summary>
		/// Initialize ignored selections for UI hightlight selection.
		/// </summary>
		void InitializeIgnoredSelection()
		{
			List<UISelectable> uiSelectableList = GetComponentsInChildren<UISelectable>(true).ToList();
			List<GameObject> goList = tabList.Select((tab) => tab.SelectButton.gameObject).ToList();

			foreach (var selectable in uiSelectableList)
			{
				selectable.AddIgnoredSelection(goList);
			}
		}

		/// <summary>
		/// Reset currently active option tab to default values.
		/// </summary>
		void IDefaultHandler.ResetToDefault()
		{
			if (!tabDictionary.TryGetValue(currentMenuTab, out Tab tab)) return;

			UISelectable.CurrentAppearSelected();

			Action yesAction = () => tab.OptionMenuUI.Default_Inspector();
			UIManager.Instance.WindowUI.OpenWindow(WindowUIType.DefaultButton, yesAction).Forget();
		}

		/// <summary>
		/// Disable all tabs which is not in ignoredTab (typically starting tab).
		/// </summary>
		/// <param name="ignoredMenuTab"></param>
		void DisableAllTabs()
		{
			foreach (var tab in tabList)
			{
				tab.OptionMenuUI.gameObject.SetActive(false);
			}
		}

		void OnDisable()
		{
			currentMenuIndex = 0;
			EventSystem.current?.SetSelectedGameObject(null);
		}
	}
}