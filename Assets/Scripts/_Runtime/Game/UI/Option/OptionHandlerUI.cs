using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using static Personal.UI.Window.WindowEnum;

namespace Personal.UI.Option
{
	public class OptionHandlerUI : MenuUIBase, IDefaultHandler
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

		[SerializeField] MenuTab startMenuTab = MenuTab.Game;
		[SerializeField] List<Tab> tabList = new();

		public IReadOnlyDictionary<MenuTab, Tab> TabDictionary { get => tabDictionary; }

		Dictionary<MenuTab, Tab> tabDictionary = new Dictionary<MenuTab, Tab>();

		MenuTab currentMenuTab;
		int currentMenuIndex;

		public override void InitialSetup()
		{
			IDefaultHandler = this;

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

					tab.SelectButton.interactable = false;
					tab.OptionMenuUI.gameObject.SetActive(true);
				});

				tabDictionary.Add(tab.OptionMenuUI.MenuTab, tab);
			}

			// Set game tab to be in a pressed state.
			tabDictionary.TryGetValue(MenuTab.Game, out Tab gameTab);
			gameTab.SelectButton.interactable = false;
		}

		public override void OpenWindow()
		{
			base.OpenWindow();

			PauseEventBegin(true);
			UIManager.Instance.FooterIconDisplay.gameObject.SetActive(true);

			DisableAllTabs();
			if (!tabDictionary.TryGetValue(startMenuTab, out Tab tab)) return;

			// Activate current menu tab.
			tab.SelectButton.interactable = false;
			tab.OptionMenuUI.gameObject.SetActive(true);
		}

		public override void CloseWindow()
		{
			if (IsWindowStackClose()) return;

			base.CloseWindow();
			currentMenuTab = startMenuTab;

			PauseEventBegin(false);
			UIManager.Instance.FooterIconDisplay.gameObject.SetActive(false);

			foreach (var tab in tabList)
			{
				tab.SelectButton.interactable = true;
				tab.OptionMenuUI.Save_Inspector();

				// Set the start menu tab.
				if (tab.OptionMenuUI.MenuTab != startMenuTab) continue;
				tab.SelectButton.interactable = false;
			}

			SaveManager.Instance.SaveProfileData();
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

		protected override void OnMainScene()
		{
			foreach (var tab in tabList)
			{
				tab.OptionMenuUI.SetDataToRelevantMember().Forget();
			}
		}

		/// <summary>
		/// Reset currently active option tab to default values.
		/// </summary>
		void IDefaultHandler.ResetToDefault()
		{
			if (!tabDictionary.TryGetValue(currentMenuTab, out Tab tab)) return;

			Action yesAction = () => tab.OptionMenuUI.Default_Inspector();
			_ = UIManager.Instance.WindowUI.OpenWindow(WindowUIType.DefaultButton, yesAction);
		}

		void DisableAllTabs()
		{
			foreach (var tab in tabList)
			{
				tab.SelectButton.interactable = true;
				tab.OptionMenuUI.gameObject.SetActive(false);
			}
		}
	}
}