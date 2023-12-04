using System;
using UnityEngine;
using UnityEngine.UI;

using Personal.UI.Option;

namespace Personal.UI
{
	[Serializable]
	public class Tab
	{
		[SerializeField] Button selectButton = null;
		[SerializeField] GameObject displayGameObject = null;
		[SerializeField] OptionMenuUI optionMenuUI = null;

		public Button SelectButton { get => selectButton; }
		public GameObject DisplayGameObject { get => displayGameObject; }
		public OptionMenuUI OptionMenuUI { get => optionMenuUI; }
	}
}
