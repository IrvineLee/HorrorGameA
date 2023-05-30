using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI.Dialog
{
	public class DialogBoxButtonPress : DialogBoxMenuUIBase
	{
		[Serializable]
		public class ButtonTextInfo
		{
			[SerializeField] Button button = null;
			[SerializeField] TextMeshProUGUI tmpText = null;
			[SerializeField] string defaultTMPText = "OK";

			public Button Button { get => button; }

			public void SetTMP(string buttonText)
			{
				tmpText.text = buttonText;
			}

			public void ResetTMP()
			{
				tmpText.text = defaultTMPText;
			}
		}

		[SerializeField] protected List<ButtonTextInfo> buttonTextInfoList = new();

		/// <summary>
		/// Add listener to button which will remove itself after pressing it.
		/// </summary>
		/// <param name="action"></param>
		public virtual void AddListenerToButtonOnce(Action action, string buttonText) { }

		/// <summary>
		/// Add listeners to buttons which will remove itself after pressing it.
		/// </summary>
		/// <param name="action01"></param>
		/// <param name="action02"></param>
		public virtual void AddListenerToButtonOnce(Action action01, Action action02, string buttonText01, string buttonText02) { }

		/// <summary>
		/// Add listeners to buttons which will remove itself after pressing it.
		/// </summary>
		/// <param name="action01"></param>
		/// <param name="action02"></param>
		/// <param name="action03"></param>
		public virtual void AddListenerToButtonOnce(Action action01, Action action02, Action action03,
													 string buttonText01, string buttonText02, string buttonText03)
		{ }

		protected virtual void SetAction(ButtonTextInfo buttonTextInfo, Action action, string buttonText)
		{
			if (buttonText != default) buttonTextInfo.SetTMP(buttonText);

			buttonTextInfo.Button.onClick.AddListener(() => action?.Invoke());
		}
	}
}
