using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	public class FooterIconDisplay : GameInitialize
	{
		[SerializeField] TextMeshProUGUI iconWithTMP = null;
		[SerializeField] Transform layoutGroupTrans = null;
		[SerializeField] int initialSpawnCount = 3;

		List<TextMeshProUGUI> iconWithTMPList = new();

		protected override void Initialize()
		{
			// Using normal instantiate for fast display.
			for (int i = 0; i < initialSpawnCount; i++)
			{
				var tmp = Instantiate(iconWithTMP, layoutGroupTrans);
				iconWithTMPList.Add(tmp);
			}

			DisableAllTmp();

			InputManager.Instance.OnDeviceIconChanged += UpdateIcons;
		}

		protected override void OnEnable()
		{
			UpdateIcons();
		}

		void UpdateIcons()
		{
			if (!UIManager.Instance.OptionUI.gameObject.activeSelf) return;

			var uiList = InputManager.Instance.ButtonIconDefinition.Ui_ButtonIconInfoList;
			DisplayIcons(InputManager.Instance.ButtonIconDefinition.GetAllText(uiList));
		}

		void DisplayIcons(List<string> textList)
		{
			HandleSpawnAdditionalGO(textList.Count);
			DisableAllTmp();

			// Enable the tmp with display icon text.
			for (int i = 0; i < textList.Count; i++)
			{
				TextMeshProUGUI tmp = iconWithTMPList[i];

				tmp.text = textList[i];
				tmp.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Spawn more instances if needed.
		/// </summary>
		/// <param name="toDisplayCount"></param>
		/// <returns></returns>
		void HandleSpawnAdditionalGO(int toDisplayCount)
		{
			if (toDisplayCount < iconWithTMPList.Count) return;

			int count = toDisplayCount - iconWithTMPList.Count;
			for (int i = 0; i < count; i++)
			{
				var tmp = Instantiate(iconWithTMP, layoutGroupTrans);
				iconWithTMPList.Add(tmp);
			}
		}

		void DisableAllTmp()
		{
			foreach (var tmp in iconWithTMPList)
			{
				tmp.gameObject.SetActive(false);
			}
		}

		// To remove the error when quitting application after InputManager has been destroyed.
		void OnApplicationQuit()
		{
			InputManager.Instance.OnDeviceIconChanged -= UpdateIcons;
		}
	}
}
