using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Personal.GameState;
using Personal.InputProcessing;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class InputManager : GameInitializeSingleton<InputManager>
	{
		[SerializeField] InputReader inputReader = null;
		[SerializeField] ActionMapType defaultActionMap = ActionMapType.Player;

		public InputReader InputReader { get => inputReader; }
		public PlayerActionInput PlayerActionInput { get; private set; }

		public FPSInputController FPSInputController { get; private set; }
		public UIInputController UIInputController { get; private set; }
		public PuzzleInputController PuzzleInputController { get; private set; }

		public ActionMapType CurrentActionMapType { get; private set; }

		public bool IsInteract
		{
			get
			{
				switch (CurrentActionMapType)
				{
					case ActionMapType.UI: return UIInputController.IsInteract;
					case ActionMapType.Puzzle: return PuzzleInputController.IsInteract;
					default: return FPSInputController.IsInteract;
				}
			}
		}

		public bool IsCancel
		{
			get
			{
				switch (CurrentActionMapType)
				{
					case ActionMapType.UI: return UIInputController.IsCancel;
					case ActionMapType.Puzzle: return PuzzleInputController.IsCancel;
					default: return FPSInputController.IsCancel;
				}
			}
		}

		// This is only used to get the current control scheme.
		// Should be able to remove this by following the link.
		// https://forum.unity.com/threads/solved-can-the-new-input-system-be-used-without-the-player-input-component.856108/#post-5669128
		public PlayerInput PlayerInput { get; private set; }

		protected override async UniTask Awake()
		{
			await base.Awake();

			FPSInputController = GetComponentInChildren<FPSInputController>();
			UIInputController = GetComponentInChildren<UIInputController>();
			PuzzleInputController = GetComponentInChildren<PuzzleInputController>();

			PlayerInput = GetComponentInChildren<PlayerInput>();

			inputReader.Initialize();
			EnableActionMap(defaultActionMap);
		}

		public void EnableActionMap(ActionMapType actionMap)
		{
			// Disable all action map.
			foreach (var map in inputReader.InputActionMapDictionary)
			{
				map.Value.InputActionMap.Disable();
				map.Value.InputController.enabled = false;
			}

			// Enable specified actin map.
			inputReader.InputActionMapDictionary.TryGetValue(actionMap, out var inputActionMap);
			inputActionMap.InputActionMap.Enable();
			inputActionMap.InputController.enabled = true;

			CurrentActionMapType = actionMap;
		}

		public void ResetToDefaultActionMap()
		{
			EnableActionMap(defaultActionMap);
		}
	}
}

