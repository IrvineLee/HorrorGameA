using UnityEngine;

using Personal.GameState;
using Personal.Save;

namespace Personal.UI.Option
{
	public class CreditsUI : MenuUIBase
	{
		[SerializeField] Transform specialThanksTrans = null;

		SaveProfile saveProfile;

		protected override void EarlyInitialize()
		{
			saveProfile = GameStateBehaviour.Instance.SaveProfile;
		}

		void OnEnable()
		{
			if (!saveProfile.UnlockedAchievementList.Contains(Achievement.AchievementType.Clear_Game_Once)) return;

			// TODO: Get your steam id name.
			specialThanksTrans.gameObject.SetActive(true);
		}
	}
}