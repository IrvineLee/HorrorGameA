using UnityEngine;

using Personal.FSM.Character;
using Personal.Manager;

namespace Personal.Character.Player
{
	public class PlayerController : ActorController
	{
		[SerializeField] PlayerStateMachine fsm = null;
		[SerializeField] FPSController fpsController = null;
		[SerializeField] PlayerInventory inventory = null;

		public PlayerStateMachine FSM { get => fsm; }
		public FPSController FPSController { get => fpsController; }
		public PlayerInventory Inventory { get => inventory; }
		public PlayerCameraView PlayerCameraView { get; private set; }

		void Awake()
		{
			StageManager.Instance.RegisterPlayer(this);
			PlayerCameraView = StageManager.Instance?.MainCamera?.GetComponentInChildren<CameraHandler>().PlayerCameraView;
		}

		public void PauseFSM(bool isFlag)
		{
			FSM.PauseStateMachine(isFlag);

			if (isFlag)
			{
				CursorManager.Instance.SetCenterCrosshair(Definition.CursorDefinition.CrosshairType.UI_Nothing);
				return;
			}
			CursorManager.Instance.SetToDefaultCenterCrosshair();
		}
	}
}