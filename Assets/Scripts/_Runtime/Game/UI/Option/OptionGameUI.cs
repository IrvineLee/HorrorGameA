using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;
using Personal.Setting.Game;
using Helper;
using Personal.Character.Player;

namespace Personal.UI.Option
{
	public class OptionGameUI : OptionMenuUI
	{
		[SerializeField] Slider brightnessSlider = null;
		[SerializeField] Slider cameraSensitivitySlider = null;

		GameData gameData;

		float currentBrightness01;

		FPSController fpsController;
		Volume volume;
		VolumeProfile volumeProfile;
		ColorAdjustments colorAdjustments;

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override async UniTask Initialize()
		{
			await base.Initialize();

			fpsController = StageManager.Instance.PlayerFSM.FPSController;

			InitializeVolumeProfile();
			HandleLoadDataToUI();
			RegisterEventsForUI();
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			base.Save_Inspector();

			gameData.Brightness = currentBrightness01;
			gameData.CameraSensitivity = cameraSensitivitySlider.value;
		}

		/// <summary>
		/// Reset data back to default.
		/// </summary>
		public override void Default_Inspector()
		{
			// Reset data.
			GameStateBehaviour.Instance.SaveProfile.OptionSavedData.ResetGameData();

			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;
			base.Default_Inspector();
		}

		/// <summary>
		/// Register events for real-time update.
		/// </summary>
		void RegisterEventsForUI()
		{
			brightnessSlider.onValueChanged.AddListener((value) =>
			{
				currentBrightness01 = value.ConvertRatio0To1();
				colorAdjustments.postExposure.value = currentBrightness01;
			});

			cameraSensitivitySlider.onValueChanged.AddListener((value) =>
			{
				cameraSensitivitySlider.value = value.Round(1);
				fpsController.UpdateRotationSpeed(cameraSensitivitySlider.value);
			});
		}

		protected override void ResetDataToUI()
		{
			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;

			brightnessSlider.value = gameData.Brightness.ConvertRatio0To100();
			cameraSensitivitySlider.value = gameData.CameraSensitivity;
		}

		protected override void ResetDataToTarget()
		{
			colorAdjustments.postExposure.value = gameData.Brightness;
			fpsController.UpdateRotationSpeed(gameData.CameraSensitivity);
		}

		void InitializeVolumeProfile()
		{
			volume = FindObjectOfType<Volume>();
			if (!volume) return;

			volumeProfile = volume.sharedProfile;
			volumeProfile.TryGet(out colorAdjustments);
		}

		void OnDestroy()
		{
			brightnessSlider.onValueChanged.RemoveAllListeners();
			cameraSensitivitySlider.onValueChanged.RemoveAllListeners();
		}
	}
}