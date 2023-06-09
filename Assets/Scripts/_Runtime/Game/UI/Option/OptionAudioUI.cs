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
		[Space]
		[SerializeField] TMP_Dropdown speakerModeDropdown = null;

		[SerializeField] Slider masterSlider = null;
		[SerializeField] Slider bgmSlider = null;
		[SerializeField] Slider sfxSlider = null;

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

		void OnEnable()
		{
			lastSelectedGO = speakerModeDropdown.gameObject;
		}

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override void InitialSetup()
		{
			HandleLoadDataToUI();
			RegisterEventsForUI();
			RegisterChangesMadeEvents();
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			base.Save_Inspector();

			// Set data volume and save data.
			audioData.SetVolume(currentMaster01, currentBgm01, currentSfx01);
			audioData.SetAudioSpeakerMode(currentSpeakerMode);
		}

		/// <summary>
		/// Reset data back to default.
		/// </summary>
		public override void Default_Inspector()
		{
			// Reset data.
			GameStateBehaviour.Instance.SaveProfile.OptionSavedData.ResetAudioData();

			audioData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.AudioData;
			base.Default_Inspector();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected override void HandleLoadDataToUI()
		{
			audioData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.AudioData;
			base.HandleLoadDataToUI();
		}

		protected override void RegisterChangesMadeEvents()
		{
			unityEventIntList.Add(speakerModeDropdown.onValueChanged);

			unityEventFloatList.Add(masterSlider.onValueChanged);
			unityEventFloatList.Add(bgmSlider.onValueChanged);
			unityEventFloatList.Add(sfxSlider.onValueChanged);

			base.RegisterChangesMadeEvents();
		}

		/// <summary>
		/// Register events for real-time update.
		/// </summary>
		void RegisterEventsForUI()
		{
			speakerModeDropdown.onValueChanged.AddListener(SpeakerModeUpdateRealtime);

			masterSlider.onValueChanged.AddListener(VolumeUpdateRealtime);
			bgmSlider.onValueChanged.AddListener(VolumeUpdateRealtime);
			sfxSlider.onValueChanged.AddListener(VolumeUpdateRealtime);
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

			// Update current value.
			currentMaster01 = masterSlider.value;
			currentBgm01 = bgmSlider.value;
			currentSfx01 = sfxSlider.value;

			currentSpeakerMode = audioData.AudioSpeakerMode;
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

		void OnApplicationQuit()
		{
			speakerModeDropdown.onValueChanged.RemoveAllListeners();

			masterSlider.onValueChanged.RemoveAllListeners();
			bgmSlider.onValueChanged.RemoveAllListeners();
			sfxSlider.onValueChanged.RemoveAllListeners();
		}
	}
}