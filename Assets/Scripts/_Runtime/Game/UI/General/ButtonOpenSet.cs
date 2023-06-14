using System;
using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	[Serializable]
	public class ButtonOpenSet
	{
		[SerializeField] Button button = null;
		[SerializeField] GameObject go = null;

		public Button Button { get => button; }
		public GameObject Go { get => go; }
	}
}
