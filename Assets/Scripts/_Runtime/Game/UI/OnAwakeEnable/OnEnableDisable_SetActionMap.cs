using UnityEngine;

using Helper;
using Personal.Manager;
using Personal.InputProcessing;
using Personal.GameState;

namespace Personal.UI
{
	public class OnEnableDisable_SetActionMap : GameInitialize
	{
		[SerializeField] ActionMapType actionMapType = ActionMapType.UI;

		ActionMapType previousActionMap;

		protected override void OnEnabled()
		{
			CoroutineHelper.WaitEndOfFrame(() =>
			{
				previousActionMap = InputManager.Instance.CurrentActionMapType;
				InputManager.Instance.EnableActionMap(actionMapType);
			});
		}

		protected override void OnDisabled()
		{
			CoroutineHelper.WaitEndOfFrame(() =>
			{
				if (previousActionMap == ActionMapType.None) return;
				InputManager.Instance?.EnableActionMap(previousActionMap);
			});
		}
	}
}
