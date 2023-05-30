using System.Collections.Generic;
using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using static Personal.UI.Dialog.DialogBoxEnum;

namespace Personal.UI.Dialog
{
	public class DialogBoxHandlerUI : MonoBehaviour
	{
		[Serializable]
		public class ButtonInfo
		{
			[SerializeField] ButtonDisplayType buttonType = ButtonDisplayType.One_Ok;
			[SerializeField] DialogBoxButtonPress buttonPress = null;

			public ButtonDisplayType ButtonType { get => buttonType; }
			public DialogBoxButtonPress ButtonPress { get => buttonPress; }
		}

		public Stack<DialogBoxMenuUI> DialogBoxStack { get; } = new();

		Dictionary<DialogUIType, DialogBoxMenuUI> dialogUIDictionary = new();

		/// <summary>
		/// Disable all dialog displays.
		/// </summary>
		public void DisableAllDialogDisplays()
		{
			foreach (var dialogBox in dialogUIDictionary)
			{
				dialogBox.Value.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Open dialog box with certain parameters.
		/// </summary>
		public async UniTask OpenDialogBox(DialogUIType dialogUIType, Action action01 = default, Action action02 = default, Action action03 = default)
		{
			var entity = MasterDataManager.Instance.MasterDialogUI.Get(dialogUIType);

			// Spawn the display dialog if it hasn't been created.
			if (!dialogUIDictionary.TryGetValue(dialogUIType, out DialogBoxMenuUI dialogBoxMenuUI))
			{
				GameObject go = await AddressableHelper.Spawn(entity.dialogDisplayType.GetStringValue(), Vector3.zero, transform);
				dialogBoxMenuUI = go.GetComponentInChildren<DialogBoxMenuUI>();

				await dialogBoxMenuUI.Initialize();
				dialogBoxMenuUI.SetSize(new Vector2(entity.widthRatio * Screen.width, entity.heightRatio * Screen.height));

				dialogUIDictionary.Add(dialogUIType, dialogBoxMenuUI);
				SetDialogButton(dialogBoxMenuUI, entity, action01, action02, action03);

				return;
			}

			// Don't do anything if the dialog is already opened.
			if (dialogBoxMenuUI.gameObject.activeSelf) return;

			DialogBoxStack.Push(dialogBoxMenuUI);
			dialogBoxMenuUI.gameObject.SetActive(true);
		}

		/// <summary>
		/// Attach buttons to the dialog.
		/// </summary>
		async void SetDialogButton(DialogBoxMenuUI dialogBoxMenuUI, DialogUIEntity entity, Action action01, Action action02, Action action03)
		{
			ButtonDisplayType buttonDisplayType = entity.buttonDisplayType;
			string title = entity.title_EN;
			string description = entity.description_EN;

			GameObject go = await AddressableHelper.Spawn(entity.buttonDisplayType.GetStringValue(), Vector3.zero, dialogBoxMenuUI.transform);
			DialogBoxButtonPress buttonPress = go.GetComponentInChildren<DialogBoxButtonPress>();

			if (buttonDisplayType == ButtonDisplayType.One_Ok)
			{
				dialogBoxMenuUI.SetOneButtonOk(buttonPress, title, description, action01);
			}
			else if (buttonDisplayType == ButtonDisplayType.Two_YesNo)
			{
				dialogBoxMenuUI.SetTwoButtonYesNo(buttonPress, title, description, action01, action02);
			}
			else if (buttonDisplayType == ButtonDisplayType.Three)
			{
				dialogBoxMenuUI.SetThreeButton(buttonPress, title, description, action01, action02, action03);
			}

			// Push it to the stack and enable it.
			DialogBoxStack.Push(dialogBoxMenuUI);
			dialogBoxMenuUI.gameObject.SetActive(true);
		}
	}
}
