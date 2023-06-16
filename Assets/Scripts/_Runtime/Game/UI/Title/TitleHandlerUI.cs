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
		[SerializeField] GameObject buttonGroup = null;

		protected override void Initialize()
		{
			InputManager.Instance.OnAnyButtonPressed += Begin;
		}

		protected override void OnUpdate()
		{
			if (EventSystem.current.currentSelectedGameObject != null) return;
			if (!lastSelectedGO) return;

			EventSystem.current.SetSelectedGameObject(lastSelectedGO);
		}

		void Begin()
		{
			Action doLast = () =>
			{
				buttonGroup.SetActive(true);
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