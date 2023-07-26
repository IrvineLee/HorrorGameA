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

		void Awake()
		{
			StageManager.Instance.RegisterPlayer(this);
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