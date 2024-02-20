using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Cysharp.Threading.Tasks;
using Helper;
using Personal.GameState;
using Personal.Save;
using Personal.Manager;
using Personal.Achievement;

namespace Personal.UI.Option
{
	public class CreditsUI : MenuUI
	{
		[SerializeField] Transform specialThanksTrans = null;
		[SerializeField] TextMeshProUGUI playerNameTMP = null;
		[SerializeField] Image image = null;
		[SerializeField] Sprite defaultSprite = null;

		SaveProfile saveProfile;

		protected override void EarlyInitialize()
		{
			saveProfile = GameStateBehaviour.Instance.SaveProfile;
		}

		async void OnEnable()
		{
			if (!saveProfile.UnlockedAchievementList.Contains(AchievementType.ClearGameOnce)) return;

			bool isStreamerMode = saveProfile.OptionSavedData.GameData.IsStreamerMode;
			string playerName = "You!!";
			string steamName = SteamManager.Instance.PlayerName;

			playerNameTMP.text = !isStreamerMode && !string.IsNullOrEmpty(steamName) ? steamName : playerName;

			await HandleAvatar(isStreamerMode);
			specialThanksTrans.gameObject.SetActive(true);
		}

		async UniTask HandleAvatar(bool isStreamerMode)
		{
			if (isStreamerMode)
			{
				image.sprite = defaultSprite;
				return;
			}

			var avatar = await SteamManager.Instance.GetAvatar();
			if (avatar != null)
			{
				Texture2D texture2D = ((Steamworks.Data.Image)avatar).Convert();
				Rect rect = new Rect(0, 0, texture2D.width, texture2D.height);
				Vector2 pivot = new Vector2(0.5f, 0.5f);

				image.sprite = Sprite.Create(texture2D, rect, pivot, 100f, 0, SpriteMeshType.FullRect);
			}
		}
	}
}