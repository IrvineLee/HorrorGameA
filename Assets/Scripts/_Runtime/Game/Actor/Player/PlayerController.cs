using UnityEngine;

using Personal.FSM.Character;
using Personal.Manager;
using Personal.Character.Animation;
using Personal.Save;
using UnityEngine.SceneManagement;

namespace Personal.Character.Player
{
	public class PlayerController : ActorController, IDataPersistence
	{
		[SerializeField] PlayerStateMachine fsm = null;
		[SerializeField] FPSController fpsController = null;
		[SerializeField] PlayerInventory inventory = null;
		[SerializeField] PlayerAnimatorController playerAnimatorController = null;
		[SerializeField] ParentMoveFollowChild parentMoveFollowChild = null;

		public PlayerStateMachine FSM { get => fsm; }
		public FPSController FPSController { get => fpsController; }
		public PlayerInventory Inventory { get => inventory; }
		public PlayerAnimatorController PlayerAnimatorController { get => playerAnimatorController; }

		protected override void EarlyInitialize()
		{
			StageManager.Instance.RegisterPlayer(this);
		}

		protected override void Initialize()
		{
			parentMoveFollowChild.enabled = true;
		}

		public void PauseFSM(bool isFlag)
		{
			FSM.PauseStateMachine(isFlag);
			FPSController.enabled = !isFlag;
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			data.PlayerSavedData.Position = transform.position;
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			// In case when the scene does not match up, load from the default position. Likely to only happen in editor mode.
			if (!data.PlayerSavedData.SceneName.Equals(SceneManager.GetActiveScene().name)) return;

			transform.position = data.PlayerSavedData.Position;
		}
	}
}