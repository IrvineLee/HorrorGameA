using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Steamworks;
using Helper;
using Personal.GameState;
using Personal.Save;
using Personal.Manager;

namespace Personal.UI.Option
{
	public class CreditsUI : MenuUIBase
	{
		[SerializeField] Transform specialThanksTrans = null;
		[SerializeField] TextMeshProUGUI playerNameTMP = null;
		[SerializeField] Image image = null;

		SaveProfile saveProfile;

		protected override void EarlyInitialize()
		{
			saveProfile = GameStateBehaviour.Instance.SaveProfile;
		}

		async void OnEnable()
		{
			if (!saveProfile.UnlockedAchievementList.Contains(Achievement.AchievementType.Clear_Game_Once)) return;
			if (specialThanksTrans.gameObject.activeSelf) return;

			playerNameTMP.text = SteamClient.Name;
			var avatar = await SteamManager.Instance.GetAvatar();

			if (avatar != null)
			{
				Texture2D texture2D = ((Steamworks.Data.Image)avatar).Convert();
				Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
				Vector2 pivot = new Vector2(0.5f, 0.5f);

				image.sprite = Sprite.Create(texture2D, rect, pivot, 100f, 0, SpriteMeshType.FullRect);
			}

			specialThanksTrans.gameObject.SetActive(true);
		}
	}
}