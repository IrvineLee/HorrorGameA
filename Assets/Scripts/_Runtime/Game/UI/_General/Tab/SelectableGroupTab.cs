using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Personal.GameState;

namespace Personal.UI
{
	public class SelectableGroupTab : GameInitialize
	{
		List<Button> buttonList = new();

		protected override void EarlyInitialize()
		{
			buttonList = GetComponentsInChildren<Button>(true).ToList();

			foreach (Button button in buttonList)
			{
				button.onClick.AddListener(() => HandleSelected(button));
			}
		}

		protected override void OnEnabled()
		{
			if (buttonList.Count <= 0) return;

			// This makes sure the first button is always selected.
			buttonList[0].onClick?.Invoke();
		}

		void HandleSelected(Button selectedButton)
		{
			foreach (Button button in buttonList)
			{
				button.interactable = true;
			}
			selectedButton.interactable = false;
		}
	}
}
