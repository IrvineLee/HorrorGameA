using UnityEngine;
using UnityEngine.SceneManagement;

using Helper;
using Personal.FSM.Character;
using Personal.Manager;
using Personal.Character.Animation;
using Personal.Save;
using Personal.InputProcessing;

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
		public InputMovement_FPSController InputMovement_FPSController { get; private set; }

		protected override void EarlyInitialize()
		{
			StageManager.Instance.RegisterPlayer(this);
			InputMovement_FPSController = GetComponentInChildren<InputMovement_FPSController>();
		}

		protected override void Initialize()
		{
			// Sometimes the player does not get set at the right position after loading.
			// This is to make sure it's properly set at the global position before setting the local position at the "parentMoveFollowChild" script.
			CoroutineHelper.WaitNextFrame(() => parentMoveFollowChild.enabled = true);
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