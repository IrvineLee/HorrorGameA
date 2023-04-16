using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

using Personal.GameState;
using Personal.Manager;
using Personal.Setting.Audio;
using Helper;
using Cysharp.Threading.Tasks;

namespace Personal.UI.Option
{
	public class OptionAudioUI : OptionMenuUI
	{
		[SerializeField] Slider masterSlider = null;
		[SerializeField] Slider bgmSlider = null;
		[SerializeField] Slider sfxSlider = null;

		[SerializeField] TMP_Dropdown speakerModeDropdown = null;

		AudioData audioData;

		float currentMaster01;
		float currentBgm01;
		float currentSfx01;

		AudioSpeakerMode currentSpeakerMode;

		// You need to make sure this configuration is the same with the dropdown menu.
		static AudioSpeakerMode[] validSpeakerModes =
		{
			AudioSpeakerMode.Stereo,
			AudioSpeakerMode.Mono,
		};

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override async UniTask Initialize()
		{
			await base.Initialize();

			HandleLoadDataToUI();
			RegisterEventsForUI();
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			// Set data volume and save data.
			audioData.SetVolume(currentMaster01, currentBgm01, currentSfx01);
			audioData.SetAudioSpeakerMode(currentSpeakerMode);

			SaveManager.Instance.SaveProfileData();
		}

		/// <summary>
		/// Reset data back to default.
		/// </summary>
		public override void Default_Inspector()
		{
			// Reset data.
			audioData = new AudioData();
			SaveManager.Instance.SaveProfileData();

			ResetDataToUI();
			ResetDataToTarget();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected override void HandleLoadDataToUI()
		{
			audioData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.AudioData;

			// Put data value to game.
			ResetDataToUI();
			ResetDataToTarget();

			// To get current value.
			currentMaster01 = masterSlider.value;
			currentBgm01 = bgmSlider.value;
			currentSfx01 = sfxSlider.value;

			currentSpeakerMode = audioData.AudioSpeakerMode;
		}

		/// <summary>
		/// Register events for real-time update.
		/// </summary>
		void RegisterEventsForUI()
		{
			masterSlider.onValueChanged.AddListener(VolumeUpdateRealtime);
			bgmSlider.onValueChanged.AddListener(VolumeUpdateRealtime);
			sfxSlider.onValueChanged.AddListener(VolumeUpdateRealtime);

			speakerModeDropdown.onValueChanged.AddListener(SpeakerModeUpdateRealtime);
		}

		/// <summary>
		/// Update the audio in real-time. Float value is not being used.
		/// </summary>
		/// <param name="value"></param>
		void VolumeUpdateRealtime(float value)
		{
			currentMaster01 = masterSlider.value.ConvertRatio0To1();
			currentBgm01 = bgmSlider.value.ConvertRatio0To1();
			currentSfx01 = sfxSlider.value.ConvertRatio0To1();

			// Set slider volume to game volume.
			SetGameVolume(currentMaster01, currentBgm01, currentSfx01);
		}

		/// <summary>
		/// Update the audio in real-time. Int value is not being used.
		/// </summary>
		/// <param name="index"></param>
		void SpeakerModeUpdateRealtime(int index)
		{
			AudioConfiguration config = AudioSettings.GetConfiguration();
			config.speakerMode = validSpeakerModes[speakerModeDropdown.value];
			AudioSettings.Reset(config);

			currentSpeakerMode = config.speakerMode;
		}

		/// <summary>
		/// Reset data to ui display.
		/// </summary>
		protected override void ResetDataToUI()
		{
			// Set data volume to slider.
			masterSlider.value = audioData.MasterVolume.ConvertRatio0To100();
			bgmSlider.value = audioData.BgmVolume.ConvertRatio0To100();
			sfxSlider.value = audioData.SfxVolume.ConvertRatio0To100();

			// Set speaker mode to dropdown.
			var list = validSpeakerModes.Select((value, index) => new { index, value })
										.Where((obj) => obj.value == audioData.AudioSpeakerMode)
										.Select((obj) => obj.index);

			speakerModeDropdown.value = list.ElementAtOrDefault(0);
		}

		/// <summary>
		/// Reset data to real audio.
		/// </summary>
		protected override void ResetDataToTarget()
		{
			SetGameVolume(audioData.MasterVolume, audioData.BgmVolume, audioData.SfxVolume);

			if (validSpeakerModes[speakerModeDropdown.value] == audioData.AudioSpeakerMode) return;

			// Reset the speaker mode.
			AudioConfiguration config = AudioSettings.GetConfiguration();
			config.speakerMode = audioData.AudioSpeakerMode;
			AudioSettings.Reset(config);
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