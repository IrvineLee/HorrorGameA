using System.Collections.Generic;
using UnityEngine;

namespace Personal.UI.Option
{
	public class OptionUI : MonoBehaviour
	{
		[SerializeField] List<GenericMenuUI> optionMenuList = null;

		public void Initialize()
		{
			foreach (var option in optionMenuList)
			{
				_ = option.Initialize();
			}
		}
	}
}