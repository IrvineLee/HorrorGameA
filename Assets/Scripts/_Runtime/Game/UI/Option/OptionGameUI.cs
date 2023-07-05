using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Personal.GameState;
using Personal.Manager;
using Personal.Setting.Game;
using Personal.InputProcessing;
using Personal.Localization;
using Helper;
using TMPro;
using Lean.Localization;

namespace Personal.UI.Option
{
	public class OptionGameUI : OptionMenuUI
	{
		[Space]
		[SerializeField] Slider brightnessSlider = null;
		[SerializeField] Slider cameraSensitivitySlider = null;
		[SerializeField] Toggle isInvertLookHorizontal = null;
		[SerializeField] Toggle isInvertLookVertical = null;
		[SerializeField] Toggle isUSInteractButton = null;
		[SerializeField] TMP_Dropdown gamepadIconDropdown = null;
		[SerializeField] TMP_Dropdown fontSizeDropdown = null;
		[SerializeField] TMP_Dropdown languageDropdown = null;

		public float CameraSensitivity { get => cameraSensitivitySlider.value; }
		public bool IsInvertLookHorizontal { get => isInvertLookHorizontal.isOn; }
		public bool IsInvertLookVertical { get => isInvertLookVertical.isOn; }

		GameData gameData;

		float currentBrightness01;

		Volume volume;
		VolumeProfile volumeProfile;
		ColorAdjustments colorAdjustments;

		DropdownLocalization dropdownLocalization;

		List<TextMeshProUGUI> allTMPList = new List<TextMeshProUGUI>();

		void OnEnable()
		{
			lastSelectedGO = brightnessSlider.gameObject;
		}

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override void InitialSetup()
		{
			dropdownLocalization = languageDropdown.GetComponentInChildren<DropdownLocalization>();

			allTMPList = PixelCrushers.DialogueSystem.DialogueManager.Instance.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
			allTMPList.AddRange(GetComponentsInChildren<TextMeshProUGUI>(true).ToList());

			InitializeVolumeProfile();
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

			gameData.Brightness = currentBrightness01;
			gameData.CameraSensitivity = cameraSensitivitySlider.value;
			gameData.IsInvertLookHorizontal = isInvertLookHorizontal.isOn;
			gameData.IsInvertLookVertical = isInvertLookVertical.isOn;
			gameData.IsUSInteractButton = isUSInteractButton.isOn;
			gameData.IconDisplayType = (IconDisplayType)gamepadIconDropdown.value;
			gameData.FontSizeType = (FontSizeType)fontSizeDropdown.value;
			gameData.SelectedLanguage = (SupportedLanguageType)languageDropdown.value;
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
			// The player events.
			cameraSensitivitySlider.onValueChanged.AddListener((value) => cameraSensitivitySlider.value = value.Round(1));

			brightnessSlider.onValueChanged.AddListener((value) =>
			{
				currentBrightness01 = value.ConvertRatio0To1();
				colorAdjustments.postExposure.value = currentBrightness01;
			});

			isUSInteractButton.onValueChanged.AddListener((value) => InputManager.Instance.SwapInteractInput(value));

			gamepadIconDropdown.onValueChanged.AddListener((value) => InputManager.Instance.SetGamepadIconIndex((IconDisplayType)value));
			fontSizeDropdown.onValueChanged.AddListener((value) => HandleFontSizeChanged((FontSizeType)value));

			languageDropdown.onValueChanged.AddListener((value) => HandleLanguageResetToTarget(value));
		}

		protected override void ResetDataToUI()
		{
			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;

			brightnessSlider.value = gameData.Brightness.ConvertRatio0To100();
			cameraSensitivitySlider.value = gameData.CameraSensitivity;

			isInvertLookHorizontal.isOn = gameData.IsInvertLookHorizontal;
			isInvertLookVertical.isOn = gameData.IsInvertLookVertical;

			isUSInteractButton.isOn = gameData.IsUSInteractButton;

			gamepadIconDropdown.value = (int)gameData.IconDisplayType;
			fontSizeDropdown.value = (int)gameData.FontSizeType;

			HandleLanguageResetToUI();
		}

		protected override void ResetDataToTarget()
		{
			colorAdjustments.postExposure.value = gameData.Brightness;

			InputManager.Instance.SwapInteractInput(gameData.IsUSInteractButton);

			InputManager.Instance.SetGamepadIconIndex(gameData.IconDisplayType);
			HandleFontSizeChanged(gameData.FontSizeType);

			HandleLanguageResetToTarget(languageDropdown.value);
		}

		protected override void RegisterChangesMadeEvents()
		{
			unityEventFloatList.Add(brightnessSlider.onValueChanged);
			unityEventFloatList.Add(cameraSensitivitySlider.onValueChanged);

			unityEventBoolList.Add(isInvertLookHorizontal.onValueChanged);
			unityEventBoolList.Add(isInvertLookVertical.onValueChanged);

			unityEventBoolList.Add(isUSInteractButton.onValueChanged);
			unityEventIntList.Add(gamepadIconDropdown.onValueChanged);
			unityEventIntList.Add(fontSizeDropdown.onValueChanged);
			unityEventIntList.Add(languageDropdown.onValueChanged);

			base.RegisterChangesMadeEvents();
		}

		void InitializeVolumeProfile()
		{
			volume = FindObjectOfType<Volume>();
			if (!volume) return;

			volumeProfile = volume.sharedProfile;
			volumeProfile.TryGet(out colorAdjustments);
		}

		void HandleFontSizeChanged(FontSizeType fontSizeType)
		{
			string textStyle = fontSizeType.GetStringValue();
			foreach (var tmp in allTMPList)
			{
				tmp.textStyle = TMP_Settings.defaultStyleSheet.GetStyle(textStyle);
			}
		}

		void HandleLanguageResetToUI()
		{
			int languageIndex = 0;
			for (int i = 0; i < dropdownLocalization.LeanLanguageList.Count; i++)
			{
				string currentLanguage = dropdownLocalization.LeanLanguageList[i];

				if (!currentLanguage.Equals(gameData.SelectedLanguage.GetStringValue())) continue;
				languageIndex = i;
			}

			languageDropdown.value = languageIndex;
		}

		void HandleLanguageResetToTarget(int index)
		{
			string language = dropdownLocalization.LeanLanguageList[index];

			// Set the UI's localization.
			LeanLocalization.SetCurrentLanguageAll(language);

			SupportedLanguageType supportedLanguageType = (SupportedLanguageType)languageDropdown.value;

			// Set the data's localization.
			MasterDataManager.Instance.Localization.UpdateActiveLanguage(supportedLanguageType);

			// Set the dialogues's localization.
			StageManager.Instance.DialogueSystemController.SetLanguage(LanguageShorthand.Get(supportedLanguageType.GetStringValue()));
		}

		void OnApplicationQuit()
		{
			brightnessSlider.onValueChanged.RemoveAllListeners();
			cameraSensitivitySlider.onValueChanged.RemoveAllListeners();
			isInvertLookHorizontal.onValueChanged.RemoveAllListeners();
			isInvertLookVertical.onValueChanged.RemoveAllListeners();
			gamepadIconDropdown.onValueChanged.RemoveAllListeners();
			fontSizeDropdown.onValueChanged.RemoveAllListeners();
		}
	}
}