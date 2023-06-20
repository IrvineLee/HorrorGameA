using System.Collections.Generic;
using UnityEngine;

using Personal.GameState;
using TMPro;
using Lean.Localization;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public class DropdownLocalization : GameInitialize
	{
		[SerializeField] List<string> leanLanguageList = new();

		public List<string> LeanLanguageList { get => leanLanguageList; }

		bool isInitialized;

		public async UniTask InitializeFontAsset()
		{
			if (isInitialized) return;
			isInitialized = true;

			await UniTask.NextFrame();

			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Transform child = transform.GetChild(i);
				if (!string.Equals(child.name, "Dropdown List")) continue;

				var tmpArray = child.GetComponentsInChildren<TextMeshProUGUI>();
				for (int j = 0; j < tmpArray.Length; j++)
				{
					// Set language font asset.
					LeanLocalization.CurrentLanguages.TryGetValue(leanLanguageList[j], out var language);
					tmpArray[j].font = language.FontAsset;
				}
				break;
			}

			// The reason why you disable it here is because once the dropdown is closed, it gets destroyed.
			// Tried doing it on destroyed script OnDestroy but it comes with problems. This is easier.
			await UniTask.NextFrame();
			isInitialized = false;
		}
	}
}
