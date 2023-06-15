using System.Collections.Generic;
using UnityEngine;

namespace Personal.Constant
{
	public class ConstantFixed : MonoBehaviour
	{
		public const string DEFINITION_PATH = "Assets/Data/Definition/";

		public const string DEFINITION_AUDIO_PATH = DEFINITION_PATH + "Audio/";
		public const string DEFINITION_CASHIER_PATH = DEFINITION_PATH + "Cashier/";

		// Audio
		public const string BGM_PATH = "BGM/";
		public const string SFX_PATH = "SFX/";

		public const float PLAYER_LOOK_SPHERECAST_RADIUS = 0.05f;
		public const float PLAYER_LOOK_SPHERECAST_LENGTH = 2f;

		// Dissolve shader
		public const float FULLY_VISIBLE_REND_VALUE = 2.5f;
		public const float FULLY_DISAPPEAR_REND_VALUE = -0.5f;

		public static IReadOnlyList<string> MAIN_SCENE_LIST = new List<string> { SceneName.Main };
	}
}