using UnityEngine;

namespace Personal.UI
{
	public class DropdownOpenedCaller : MonoBehaviour
	{
		DropdownLocalization dropdownLocalization;

		void Awake()
		{
			dropdownLocalization = GetComponentInParent<DropdownLocalization>();
		}

		void OnDisable()
		{
			dropdownLocalization?.InitializeFontAsset();
		}
	}
}
