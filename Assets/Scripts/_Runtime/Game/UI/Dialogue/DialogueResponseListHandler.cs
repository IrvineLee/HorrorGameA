using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Helper;

namespace Personal.Dialogue
{
	public class DialogueResponseListHandler : MonoBehaviour
	{
		public int SelectedResponse { get => selectedButton; set => selectedButton = value; }

		List<Button> buttonList = new();
		int selectedButton = -1;

		void OnEnable()
		{
			CoroutineHelper.WaitNextFrame(Cache);
		}

		void Cache()
		{
			ResetButtons();

			buttonList = GetComponentsInChildren<Button>().ToList();
			foreach (var button in buttonList)
			{
				button.onClick.AddListener(() => selectedButton = button.transform.GetSiblingIndex());
			}
		}

		/// <summary>
		/// Reset the buttons. 
		/// </summary>
		void ResetButtons()
		{
			foreach (var button in buttonList)
			{
				button.onClick.RemoveAllListeners();
			}

			buttonList.Clear();
			selectedButton = -1;
		}
	}
}
