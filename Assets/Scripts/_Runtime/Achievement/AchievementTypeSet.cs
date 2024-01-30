using UnityEngine;

namespace Personal.Achievement
{
	public class AchievementTypeSet : MonoBehaviour
	{
		[SerializeField] AchievementType achievementType = AchievementType.Good_Ending;

		public AchievementType AchievementType { get => achievementType; }
	}
}