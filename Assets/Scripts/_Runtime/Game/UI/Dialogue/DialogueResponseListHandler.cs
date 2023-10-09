using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Helper;

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

		void OnEnable()
		{
			// You have to wait for the dialogue response to get populated.
			CoroutineHelper.WaitNextFrame(Cache);
		}

		void Cache()
		{
			ResetButtons();

			var buttonList = GetComponentsInChildren<Button>().ToList();
			foreach (var button in buttonList)
			{
				UnityAction unityAction = () => selectedButton = button.transform.GetSiblingIndex();
				buttonActionList.Add(new ButtonUnityAction(button, unityAction));

				button.onClick.AddListener(unityAction);
			}
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
