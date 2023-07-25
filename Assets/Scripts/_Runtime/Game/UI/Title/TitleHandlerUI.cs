using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Helper;
using System.Linq;

namespace Personal.UI.Option
{
	public class TitleHandlerUI : UIHandlerBase
	{
		[SerializeField] OnEnableFadeInOut pressAnyButton = null;
		[SerializeField] Transform buttonGroupTrans = null;
		[SerializeField] CreditsUI creditsUI = null;

		[Tooltip("How long \"Press Any Button\" remain on screen after pressing button")]
		[SerializeField] float anyButtonWaitDuration = 0.5f;

		UIGamepadMovement uiGamepadMovement;

		protected override async void Initialize()
		{
			base.Initialize();
			await UniTask.WaitUntil(() => !StageManager.Instance.IsBusy);

			InitialSetup();
			uiGamepadMovement = GetComponentInChildren<UIGamepadMovement>(true);

			InputManager.Instance.SetToDefaultActionMap();
			InputManager.OnAnyButtonPressed += Begin;

			pressAnyButton.gameObject.SetActive(true);
		}

		/// <summary>
		/// Inspector set.
		/// </summary>
		public void OpenCredits()
		{
			uiGamepadMovement.SetIsUpdate(false);
			creditsUI.SetOnDisableAction(() => uiGamepadMovement.SetIsUpdate(true));
			creditsUI.OpenWindow();
		}

		void Begin()
		{
			pressAnyButton.StopFadeAndSetFullVisibility(anyButtonWaitDuration, () => buttonGroupTrans.gameObject.SetActive(true));
			RemoveListener();
		}

		void RemoveListener()
		{
			InputManager.OnAnyButtonPressed -= Begin;
		}

		void OnApplicationQuit()
		{
			RemoveListener();
		}
	}
}