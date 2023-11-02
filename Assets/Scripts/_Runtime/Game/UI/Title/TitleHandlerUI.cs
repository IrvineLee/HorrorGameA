using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Helper;

namespace Personal.UI.Option
{
	public class TitleHandlerUI : UIHandlerBase
	{
		[SerializeField] OnEnableFadeInOut pressAnyButton = null;
		[SerializeField] Transform buttonGroupTrans = null;
		[SerializeField] CreditsUI creditsUI = null;

		[Tooltip("How long \"Press Any Button\" remain on screen after pressing button")]
		[SerializeField] float anyButtonWaitDuration = 0.5f;

		protected override async void Initialize()
		{
			base.Initialize();
			await UniTask.WaitUntil(() => !StageManager.Instance.IsBusy, cancellationToken: this.GetCancellationTokenOnDestroy());

			InitialSetup();

			InputManager.Instance.SetToDefaultActionMap();
			InputManager.OnAnyButtonPressed += Begin;

			pressAnyButton.gameObject.SetActive(true);
		}

		/// <summary>
		/// Inspector set.
		/// </summary>
		public void OpenCredits()
		{
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