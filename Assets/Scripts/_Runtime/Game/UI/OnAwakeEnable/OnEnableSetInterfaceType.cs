using UnityEngine;

using Personal.Manager;

namespace Personal.UI
{
	public class OnEnableSetInterfaceType : MonoBehaviour
	{
		[SerializeField] UIInterfaceType uiInterfaceType = UIInterfaceType.None;

		void OnEnable()
		{
			UIManager.Instance?.SetActiveInterfaceType(uiInterfaceType);
		}

		void OnDisable()
		{
			UIManager.Instance?.SetActiveInterfaceType(UIInterfaceType.None);
		}
	}
}
