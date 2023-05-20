using System.Collections.Generic;
using UnityEngine;

using Personal.GameState;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

using Personal.InputProcessing;

namespace Personal.Manager
{
	public class InputManager : GameInitializeSingleton<InputManager>
	{
		[SerializeField] InputReader inputReader = null;

		public InputReader InputReader { get => inputReader; }
		public PlayerActionInput PlayerActionInput { get; private set; }
		public FPSInputController FPSInputController { get; private set; }

		// This is only used to get the current control scheme.
		// Should be able to remove this by following the link.
		// https://forum.unity.com/threads/solved-can-the-new-input-system-be-used-without-the-player-input-component.856108/#post-5669128
		public PlayerInput PlayerInput { get; private set; }

		protected override async UniTask Awake()
		{
			await base.Awake();

			inputReader.Initialize();
			FPSInputController = GetComponentInChildren<FPSInputController>();
			PlayerInput = GetComponentInChildren<PlayerInput>();
		}

		public void EnableActionMap(ActionMapType actionMap)
		{
			// Disable all action map.
			foreach (var map in inputReader.InputActionMapDictionary)
			{
				map.Value.Disable();
			}

			// Enable specified actin map.
			inputReader.InputActionMapDictionary.TryGetValue(actionMap, out InputActionMap inputActionMap);
			inputActionMap.Enable();
		}
	}
}

