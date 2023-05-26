using System;
using UnityEngine;

namespace Personal.Setting.Game
{
	[Serializable]
	public class GameData
	{
		[SerializeField] float brightness = 1;
		[SerializeField] float cameraSensitivity = 1;
		[SerializeField] bool isSubtitle = true;

		public float Brightness { get => brightness; set => brightness = value; }
		public float CameraSensitivity { get => cameraSensitivity; set => cameraSensitivity = value; }
		public bool IsSubtitle { get => isSubtitle; set => isSubtitle = value; }
	}
}