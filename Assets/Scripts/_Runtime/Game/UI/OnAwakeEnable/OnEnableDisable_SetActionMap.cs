using UnityEngine;

using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.UI
{
	public class OnEnableDisable_SetActionMap : MonoBehaviour
	{
		[SerializeField] ActionMapType actionMapType = ActionMapType.BasicControl;

		ActionMapType previousActionMap;

		void OnEnable()
		{
			previousActionMap = InputManager.Instance.CurrentActionMapType;
			InputManager.Instance.EnableActionMap(actionMapType);
		}

		void OnDisable()
		{
			if (previousActionMap == ActionMapType.None) return;
			InputManager.Instance?.EnableActionMap(previousActionMap);
		}
	}
}
