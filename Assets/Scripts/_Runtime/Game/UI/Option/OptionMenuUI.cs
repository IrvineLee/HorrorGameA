using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Helper;
using Personal.InputProcessing;
using static Personal.UI.Option.OptionHandlerUI;

namespace Personal.UI.Option
{
	public class OptionMenuUI : MenuUI
	{
		[SerializeField] MenuTab menuTab = MenuTab.Graphic;

		public MenuTab MenuTab { get => menuTab; }

		public static bool IsChangesMade { get; private set; }

		protected List<UnityEvent<int>> unityEventIntList = new();
		protected List<UnityEvent<float>> unityEventFloatList = new();
		protected List<UnityEvent<bool>> unityEventBoolList = new();

		protected List<UISelectable> uiSelectableList = new();
		protected AutoScrollRect autoScrollRect;

		protected override void OnEnabled()
		{
			((BasicControllerUI)ControlInputBase.ActiveControlInput)?.SetUIValues(uiSelectableList, autoScrollRect);
		}

		public override void InitialSetup()
		{
			uiSelectableList = GetComponentsInChildren<UISelectable>(true).ToList();
			autoScrollRect = GetComponentInChildren<AutoScrollRect>(true);
		}

		/// <summary>
		/// Pressing the OK button.
		/// </summary>
		public virtual void Save_Inspector() { }

		/// <summary>
		/// Pressing the Default button.
		/// </summary>
		public virtual void Default_Inspector()
		{
			ResetData();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected virtual void HandleLoadDataToUI()
		{
			ResetData();
		}

		/// <summary>
		/// Reset the data value back to the UI.
		/// </summary>
		protected virtual void ResetDataToUI() { }

		/// <summary>
		/// Reset the data value back to the real target. Ex: Audio, Graphic etc...
		/// </summary>
		protected virtual void ResetDataToTarget() { }

		/// <summary>
		/// This is used to check whether any changes has been made to the options menu, 
		/// to see whether to save the new data or just leave it as it is.
		/// </summary>
		protected virtual void RegisterChangesMadeEvents()
		{
			foreach (var unityEvent in unityEventIntList)
			{
				RegisterChangesMade(unityEvent);
			}

			foreach (var unityEvent in unityEventFloatList)
			{
				RegisterChangesMade(unityEvent);
			}

			foreach (var unityEvent in unityEventBoolList)
			{
				RegisterChangesMade(unityEvent);
			}
		}

		void RegisterChangesMade<T>(UnityEvent<T> unityEvent)
		{
			unityEvent.AddListener((result) => IsChangesMade = true);
		}

		void UnRegisterChangesMade<T>(UnityEvent<T> unityEvent)
		{
			unityEvent.RemoveAllListeners();
		}

		void ResetData()
		{
			ResetDataToUI();
			ResetDataToTarget();
		}

		protected override void OnDisabled()
		{
			base.OnDisabled();
			IsChangesMade = false;
		}

		void OnApplicationQuit()
		{
			foreach (var unityEvent in unityEventIntList)
			{
				UnRegisterChangesMade(unityEvent);
			}

			foreach (var unityEvent in unityEventFloatList)
			{
				UnRegisterChangesMade(unityEvent);
			}

			foreach (var unityEvent in unityEventBoolList)
			{
				UnRegisterChangesMade(unityEvent);
			}
		}
	}
}
