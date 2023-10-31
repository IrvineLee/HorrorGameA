using UnityEngine;

using Personal.Manager;

namespace Personal.Achievement
{
	public class AchievementTypeSet : MonoBehaviour
	{
		[SerializeField] AchievementType achievementType = AchievementType.Test_Achievement;

		public AchievementType AchievementType { get => achievementType; }

		public void UpdateData()
		{
			AchievementManager.Instance.UpdateData(achievementType);
		}
	}
}