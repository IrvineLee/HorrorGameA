using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Personal.GameState;
using Personal.Manager;
using Personal.Setting.Audio;
using Helper;

namespace Personal.UI.Option
{
	public class AudioUI : MonoBehaviour
	{
		[SerializeField] Slider masterSlider = null;
		[SerializeField] Slider bgmSlider = null;
		[SerializeField] Slider sfxSlider = null;

		AudioData audioData;

		IEnumerator Start()
		{
			yield return new WaitUntil(() => GameManager.Instance.IsLoadingOver);

			// Set data volume to game volume.
			audioData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.AudioData;
			SetGameVolume(audioData.MasterVolume, audioData.BgmVolume, audioData.SfxVolume);

			// Set data volume to slider.
			masterSlider.value = audioData.MasterVolume.ConvertRatio0To100();
			bgmSlider.value = audioData.BgmVolume.ConvertRatio0To100();
			sfxSlider.value = audioData.SfxVolume.ConvertRatio0To100();
		}

		public void Save()
		{
			float master01 = masterSlider.value.ConvertRatio0To1();
			float bgm01 = bgmSlider.value.ConvertRatio0To1();
			float sfx01 = sfxSlider.value.ConvertRatio0To1();

			// Set slider volume to game volume.
			SetGameVolume(master01, bgm01, sfx01);

			// Set data volume and save data.
			audioData.SetVolume(master01, bgm01, sfx01);
			SaveManager.Instance.SaveProfileData();
		}

		/// <summary>
		/// Set game volume.
		/// </summary>
		/// <param name="master01"></param>
		/// <param name="bgm01"></param>
		/// <param name="sfx01"></param>
		void SetGameVolume(float master01, float bgm01, float sfx01)
		{
			// Set game volume.
			AudioManager.Instance.Bgm.volume = bgm01 * master01;
			AudioManager.Instance.Sfx.volume = sfx01 * master01;
		}
	}
}