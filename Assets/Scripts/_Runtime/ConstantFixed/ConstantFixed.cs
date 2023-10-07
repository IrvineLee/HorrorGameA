using System.Collections.Generic;
using UnityEngine;

using Helper;

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

		// UI Gamepad selection
		public const float UI_SELECTION_DELAY = 0.15f;
		public const float UI_SCROLLBAR_DURATION = 0.1f;

		// Item
		public const int ITEM_START = 10000;
		public const int ITEM_END = 19999;

		// Quest
		public const int MAIN_QUEST_START = 20000;
		public const int MAIN_QUEST_END = 24999;
		public const int SUB_QUEST_START = 25000;
		public const int SUB_QUEST_END = 29999;
	}
}