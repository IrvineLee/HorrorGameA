using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Helper;
using Personal.InputProcessing;
using Personal.UI;
using Personal.GameState;

namespace Personal.Dialogue
{
	public class DialogueResponseListHandler : GameInitialize
	{
		public int SelectedResponse { get; private set; } = 0;

		List<DialogueResponseInfo> dialogueResponseInfoList = new();

		Transform contentRectTransform;
		AutoScrollRect autoScrollRect;

		int currentTotalResponse;       // This will continue to add up until ResetSelectionResponse() is called.

		protected override void Initialize()
		{
			contentRectTransform = GetComponentInChildren<ScrollRect>(true).content;
			autoScrollRect = GetComponentInChildren<AutoScrollRect>(true);

			DialogueSetup.OnConversationEndEvent += ResetSelectedResponse;
		}

		void OnEnable()
		{
			// You have to wait for the dialogue response to get populated.
			CoroutineHelper.WaitNextFrame(InitButtons);
		}

		void InitButtons()
		{
			if (contentRectTransform.childCount <= 0) return;

			ResetButtons();
			dialogueResponseInfoList = contentRectTransform.GetComponentsInChildren<DialogueResponseInfo>().ToList();

			// Setup the new buttons to display to user.
			for (int i = dialogueResponseInfoList.Count - 1; i >= 0; i--)
			{
				SetupButton(dialogueResponseInfoList[i], i, dialogueResponseInfoList.Count);
			}

			// Update the ui values for gamepad scroll and display.
			var uiSelectableList = contentRectTransform.GetComponentsInChildren<UISelectable>().ToList();
			((BasicControllerUI)ControlInputBase.ActiveControlInput).SetUIValues(uiSelectableList, autoScrollRect);
		}

		void SetupButton(DialogueResponseInfo dialogueResponseInfo, int index, int count)
		{
			UnityAction unityAction = () =>
			{
				SelectedResponse += index;

				// Wait for the quest check dialogue response before updating the selection index.
				CoroutineHelper.WaitNextFrame(() =>
				{
					currentTotalResponse += count;
					SelectedResponse = currentTotalResponse;
				}, isEndOfFrame: true); ;
			};
			dialogueResponseInfo.SetupButton(unityAction);
		}

		/// <summary>
		/// Reset the buttons. 
		/// </summary>
		void ResetButtons()
		{
			foreach (var responseInfo in dialogueResponseInfoList)
			{
				responseInfo.ResetButton();
			}
		}

		/// <summary>
		/// Reset the selection index after dialogue is finished.
		/// </summary>
		void ResetSelectedResponse()
		{
			// Wait for the quest check dialogue response before updating the selection index.
			CoroutineHelper.WaitNextFrame(() =>
			{
				SelectedResponse = 0;
				currentTotalResponse = 0;
			}, isEndOfFrame: true);
		}

		void OnDestroy()
		{
			DialogueSetup.OnConversationEndEvent -= ResetSelectedResponse;
		}
	}
}