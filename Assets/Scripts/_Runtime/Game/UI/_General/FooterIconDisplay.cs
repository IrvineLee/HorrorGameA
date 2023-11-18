using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Personal.GameState;
using Personal.Manager;
using Personal.UI.Option;

namespace Personal.UI
{
	public class FooterIconDisplay : GameInitialize
	{
		[SerializeField] TextMeshProUGUI iconWithTMP = null;
		[SerializeField] Transform layoutGroupTrans = null;

		List<TextMeshProUGUI> iconWithTMPList = new();

		protected override void Initialize()
		{
			InputManager.OnDeviceIconChanged += UpdateIcons;
			OptionGameUI.OnXInteractEvent += UpdateInteractIcon;

			UpdateIcons();
		}

		void OnEnable()
		{
			UpdateIcons();
		}

		public void Begin(bool isFlag)
		{
			gameObject.SetActive(isFlag);
		}

		void UpdateInteractIcon(bool isXInteract)
		{
			DisplayIcons(InputManager.Instance.ButtonIconDefinition.GetCurrentInterfaceText(UIManager.Instance.ActiveInterfaceType, isXInteract));
		}

		void UpdateIcons()
		{
			DisplayIcons(InputManager.Instance.ButtonIconDefinition.GetCurrentInterfaceText(UIManager.Instance.ActiveInterfaceType));
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

		void OnApplicationQuit()
		{
			InputManager.OnDeviceIconChanged -= UpdateIcons;
			OptionGameUI.OnXInteractEvent -= UpdateInteractIcon;
		}
	}
}
