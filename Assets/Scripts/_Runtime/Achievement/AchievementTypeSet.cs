using UnityEngine;

namespace Personal.Achievement
{
	public class AchievementTypeSet : MonoBehaviour
	{
		[SerializeField] AchievementType achievementType = AchievementType.GoodEnding;

		public AchievementType AchievementType { get => achievementType; }
	}
}