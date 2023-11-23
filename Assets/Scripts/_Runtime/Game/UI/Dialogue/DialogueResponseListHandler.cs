using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Helper;
using Personal.InputProcessing;
using Personal.UI;

namespace Personal.Dialogue
{
	public class DialogueResponseListHandler : MonoBehaviour
	{
		class ButtonUnityAction
		{
			[SerializeField] Button button = null;
			[SerializeField] UnityAction unityAction = null;

			public Button Button { get => button; }
			public UnityAction UnityAction { get => unityAction; }

			public ButtonUnityAction(Button button, UnityAction unityAction)
			{
				this.button = button;
				this.unityAction = unityAction;
			}
		}

		public int SelectedResponse { get => selectedButton; set => selectedButton = value; }

		List<ButtonUnityAction> buttonActionList = new();
		int selectedButton = -1;

		Transform contentRectTransform;
		AutoScrollRect autoScrollRect;

		void Awake()
		{
			contentRectTransform = GetComponentInChildren<ScrollRect>().content;
			autoScrollRect = GetComponentInChildren<AutoScrollRect>(true);
		}

		void OnEnable()
		{
			// You have to wait for the dialogue response to get populated.
			CoroutineHelper.WaitNextFrame(Cache);
		}

		void Cache()
		{
			if (contentRectTransform.childCount <= 0) return;

			ResetButtons();

			var buttonList = contentRectTransform.GetComponentsInChildren<Button>().ToList();
			for (int i = 0; i < buttonList.Count; i++)
			{
				Button button = buttonList[i];
				int index = i;

				if (i == 0) EventSystem.current.SetSelectedGameObject(button.gameObject);

				UnityAction unityAction = () => selectedButton = index;
				buttonActionList.Add(new ButtonUnityAction(button, unityAction));

				button.onClick.AddListener(unityAction);
			}

			var uiSelectableList = contentRectTransform.GetComponentsInChildren<UISelectable>().ToList();
			((BasicControllerUI)ControlInputBase.ActiveControlInput).SetUIValues(uiSelectableList, autoScrollRect);
		}

		/// <summary>
		/// Reset the buttons. 
		/// </summary>
		void ResetButtons()
		{
			foreach (var buttonAction in buttonActionList)
			{
				buttonAction.Button.onClick.RemoveListener(buttonAction.UnityAction);
			}

			buttonActionList.Clear();
			selectedButton = -1;
		}
	}
}
