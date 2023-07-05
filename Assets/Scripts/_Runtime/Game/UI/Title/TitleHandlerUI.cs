using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Personal.Manager;
using Helper;

namespace Personal.UI.Option
{
	public class TitleHandlerUI : MenuUIBase
	{
		[SerializeField] OnEnableFadeInOut pressAnyButton = null;
		[SerializeField] Transform buttonGroupTrans = null;

		[Tooltip("How long \"Press Any Button\" remain on screen after pressing button")]
		[SerializeField] float anyButtonWaitDuration = 0.5f;

		protected override void Initialize()
		{
			InputManager.Instance?.SetToDefaultActionMap();
			InputManager.Instance.OnAnyButtonPressed += Begin;

			List<ButtonInteractBase> buttonInteractList = buttonGroupTrans.GetComponentsInChildren<ButtonInteractBase>(true).ToList();
			foreach (var buttonInteract in buttonInteractList)
			{
				buttonInteract.InitialSetup();
			}
		}

		void Begin()
		{
			pressAnyButton.StopFadeAndSetFullVisibility(anyButtonWaitDuration, () => buttonGroupTrans.gameObject.SetActive(true));
			RemoveListener();
		}

		void RemoveListener()
		{
			if (InputManager.Instance)
				InputManager.Instance.OnAnyButtonPressed -= Begin;
		}

		void OnApplicationQuit()
		{
			RemoveListener();
		}
	}
}