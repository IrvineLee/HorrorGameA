using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

using Personal.Manager;
using Helper;
using UnityEngine.EventSystems;

namespace Personal.UI.Option
{
	public class TitleHandlerUI : MenuUIBase
	{
		[SerializeField] OnEnableFadeInOut pressAnyButton = null;
		[SerializeField] Transform buttonGroupTrans = null;

		protected override void Initialize()
		{
			InputManager.Instance.OnAnyButtonPressed += Begin;

			List<ButtonInteractBase> buttonInteractList = buttonGroupTrans.GetComponentsInChildren<ButtonInteractBase>(true).ToList();
			foreach (var buttonInteract in buttonInteractList)
			{
				buttonInteract.InitialSetup();
			}
		}

		protected override void OnUpdate()
		{
			if (EventSystem.current.currentSelectedGameObject) return;
			if (!lastSelectedGO) return;

			EventSystem.current.SetSelectedGameObject(lastSelectedGO);
		}

		void Begin()
		{
			Action doLast = () =>
			{
				buttonGroupTrans.gameObject.SetActive(true);
				RemoveListener();
			};
			pressAnyButton.StopFadeAndSetFullVisibility(doLast);
		}

		void RemoveListener()
		{
			InputManager.Instance.OnAnyButtonPressed -= Begin;
		}

		void OnApplicationQuit()
		{
			RemoveListener();
		}
	}
}