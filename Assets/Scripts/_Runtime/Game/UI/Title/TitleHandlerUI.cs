using UnityEngine;

using Personal.Manager;
using Helper;

namespace Personal.UI.Option
{
	public class TitleHandlerUI : UIHandlerBase
	{
		[SerializeField] OnEnableFadeInOut pressAnyButton = null;
		[SerializeField] Transform buttonGroupTrans = null;

		[Tooltip("How long \"Press Any Button\" remain on screen after pressing button")]
		[SerializeField] float anyButtonWaitDuration = 0.5f;

		protected override void Initialize()
		{
			base.Initialize();

			InitialSetup();
			InputManager.Instance.SetToDefaultActionMap();
			InputManager.OnAnyButtonPressed += Begin;
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