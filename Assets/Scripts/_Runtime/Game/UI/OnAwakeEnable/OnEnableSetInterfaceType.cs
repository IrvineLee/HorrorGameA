using UnityEngine;

using Personal.Manager;

namespace Personal.UI
{
	public class OnEnableSetInterfaceType : MonoBehaviour
	{
		[SerializeField] UIInterfaceType uiInterfaceType = UIInterfaceType.None;

		void OnEnable()
		{
			UIManager.Instance.AddToInterfaceTypeStack(true, uiInterfaceType);
		}

		void OnDisable()
		{
			UIManager.Instance.AddToInterfaceTypeStack(false);
		}
	}
}
