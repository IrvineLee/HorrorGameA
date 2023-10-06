using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Personal.GameState;
using PixelCrushers.DialogueSystem;
using Personal.Manager;
using Personal.Setting.Game;
using Personal.InputProcessing;
using Personal.Localization;
using Personal.Dialogue;
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
		[SerializeField] UISelectionToggle isInvertLookHorizontal = null;
		[SerializeField] UISelectionToggle isInvertLookVertical = null;
		[SerializeField] UISelectionToggle isUSInteractButton = null;
		[SerializeField] UISelectionDropdown gamepadIconDropdown = null;
		[SerializeField] UISelectionDropdown fontSizeDropdown = null;
		[SerializeField] UISelectionDropdown languageDropdown = null;

		GameData gameData;

		float currentBrightness01;

		Volume volume;
		VolumeProfile volumeProfile;
		ColorAdjustments colorAdjustments;

		DialogueSetup dialogueSetup;

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
			GetComponentsInChildren<UISelectionBase>()?.ToList().ForEach(result => result.Initialize());
			dialogueSetup = DialogueManager.Instance.GetComponentInChildren<DialogueSetup>();

			allTMPList = DialogueManager.Instance.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
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
			gameData.IsInvertLookHorizontal = isInvertLookHorizontal.IsOn;
			gameData.IsInvertLookVertical = isInvertLookVertical.IsOn;
			gameData.IsUSInteractButton = isUSInteractButton.IsOn;
			gameData.IconDisplayType = (IconDisplayType)gamepadIconDropdown.Value;
			gameData.FontSizeType = (FontSizeType)fontSizeDropdown.Value;
			gameData.SelectedLanguage = (SupportedLanguageType)languageDropdown.Value;
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

			isUSInteractButton.OnValueChanged.AddListener((value) =>
			{
				InputManager.Instance.SwapInteractInput(value);
				dialogueSetup.SwapInteractInput(value);
			});

			gamepadIconDropdown.OnValueChanged.AddListener((value) => InputManager.Instance.SetGamepadIconIndex((IconDisplayType)value));
			fontSizeDropdown.OnValueChanged.AddListener((value) => HandleFontSizeChanged((FontSizeType)value));

			languageDropdown.OnValueChanged.AddListener((value) => HandleLanguageResetToTarget(value));
		}

		protected override void ResetDataToUI()
		{
			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;

			brightnessSlider.value = gameData.Brightness.ConvertRatio0To100();
			cameraSensitivitySlider.value = gameData.CameraSensitivity;

			isInvertLookHorizontal.SetCurrentIndex(gameData.IsInvertLookHorizontal ? 1 : 0);
			isInvertLookVertical.SetCurrentIndex(gameData.IsInvertLookVertical ? 1 : 0);

			isUSInteractButton.SetCurrentIndex(gameData.IsUSInteractButton ? 1 : 0);

			gamepadIconDropdown.SetCurrentIndex((int)gameData.IconDisplayType);
			fontSizeDropdown.SetCurrentIndex((int)gameData.FontSizeType);

			HandleLanguageResetToUI();
		}

		protected override void ResetDataToTarget()
		{
			colorAdjustments.postExposure.value = gameData.Brightness;

			InputManager.Instance.SwapInteractInput(gameData.IsUSInteractButton);
			dialogueSetup.SwapInteractInput(gameData.IsUSInteractButton);

			InputManager.Instance.SetGamepadIconIndex(gameData.IconDisplayType);
			HandleFontSizeChanged(gameData.FontSizeType);

			HandleLanguageResetToTarget(languageDropdown.Value);
		}

		protected override void RegisterChangesMadeEvents()
		{
			unityEventFloatList.Add(brightnessSlider.onValueChanged);
			unityEventFloatList.Add(cameraSensitivitySlider.onValueChanged);

			unityEventBoolList.Add(isInvertLookHorizontal.OnValueChanged);
			unityEventBoolList.Add(isInvertLookVertical.OnValueChanged);

			unityEventBoolList.Add(isUSInteractButton.OnValueChanged);
			unityEventIntList.Add(gamepadIconDropdown.OnValueChanged);
			unityEventIntList.Add(fontSizeDropdown.OnValueChanged);
			unityEventIntList.Add(languageDropdown.OnValueChanged);

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
			string textStyle = fontSizeType.ToString();
			foreach (var tmp in allTMPList)
			{
				tmp.textStyle = TMP_Settings.defaultStyleSheet.GetStyle(textStyle);
			}
		}

		void HandleLanguageResetToUI()
		{
			int languageIndex = 0;
			for (int i = 0; i < languageDropdown.StringList.Count; i++)
			{
				string language = languageDropdown.StringList[i];

				if (!language.Equals(gameData.SelectedLanguage.ToString())) continue;
				languageIndex = i;
			}

			languageDropdown.SetCurrentIndex(languageIndex);
		}

		void HandleLanguageResetToTarget(int index)
		{
			string language = languageDropdown.StringList[index];

			// Set the UI's localization.
			LeanLocalization.SetCurrentLanguageAll(language);

			SupportedLanguageType supportedLanguageType = (SupportedLanguageType)languageDropdown.Value;

			// Set the data's localization.
			MasterLocalization.SetActiveLanguage(supportedLanguageType);

			// Set the dialogues's localization.
			StageManager.Instance.DialogueSystemController.SetLanguage(LanguageShorthand.Get(supportedLanguageType.ToString()));
		}

		void OnApplicationQuit()
		{
			brightnessSlider.onValueChanged.RemoveAllListeners();
			cameraSensitivitySlider.onValueChanged.RemoveAllListeners();
			isInvertLookHorizontal.OnValueChanged.RemoveAllListeners();
			isInvertLookVertical.OnValueChanged.RemoveAllListeners();
			gamepadIconDropdown.OnValueChanged.RemoveAllListeners();
			fontSizeDropdown.OnValueChanged.RemoveAllListeners();
		}
	}
}