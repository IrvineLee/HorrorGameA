using UnityEngine;

namespace Personal.Achievement
{
	public class AchievementTypeSet : MonoBehaviour
	{
		[SerializeField] AchievementType achievementType = AchievementType.Test_Achievement;

		public AchievementType AchievementType { get => achievementType; }
	}
}