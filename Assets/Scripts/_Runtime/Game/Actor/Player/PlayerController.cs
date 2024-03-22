using UnityEngine;
using UnityEngine.SceneManagement;

using Cinemachine;
using Cysharp.Threading.Tasks;
using Helper;
using Personal.FSM.Character;
using Personal.Manager;
using Personal.Character.Animation;
using Personal.Save;
using Personal.InputProcessing;
using Personal.FSM;

namespace Personal.Character.Player
{
	public class PlayerController : ActorController, IDataPersistence
	{
		[SerializeField] PlayerStateMachine fsm = null;
		[SerializeField] FPSController fpsController = null;
		[SerializeField] PlayerInventory inventory = null;
		[SerializeField] PlayerModel modelController = null;
		[SerializeField] ParentMoveFollowChild parentMoveFollowChild = null;

		public PlayerStateMachine FSM { get => fsm; }
		public FPSController FPSController { get => fpsController; }
		public PlayerInventory Inventory { get => inventory; }
		public PlayerModel ModelController { get => modelController; }

		public InputMovement_FPSController InputMovement_FPSController { get; private set; }
		public PlayerScanAOE PlayerScanAOE { get; private set; }
		public PlayerAnimationAudio PlayerAnimationAudio { get; private set; }
		public Animator Animator { get; private set; }

		CharacterController characterController;
		CoroutineRun rotateBodyCR = new();

		protected override void EarlyInitialize()
		{
			StageManager.Instance.RegisterPlayer(this);

			InputMovement_FPSController = GetComponentInChildren<InputMovement_FPSController>();
			PlayerScanAOE = GetComponentInChildren<PlayerScanAOE>();
			PlayerAnimationAudio = GetComponentInChildren<PlayerAnimationAudio>();
			Animator = GetComponentInChildren<Animator>();
			characterController = GetComponentInChildren<CharacterController>();

			var animator = GetComponentInChildren<Animator>();
			animator.enabled = false;
		}

		protected override void Initialize()
		{
			// Sometimes the player does not get set at the right position after loading.
			// This is to make sure it's properly set at the global position before setting the local position at the "parentMoveFollowChild" script.
			CoroutineHelper.WaitNextFrame(() => parentMoveFollowChild.enabled = true);
		}

		public void PauseControl(bool isFlag)
		{
			FPSController.enabled = !isFlag;
			modelController.AnimatorController.ResetAnimationBlend(0.25f);
			PlayerScanAOE.EnableScan(!isFlag);
		}

		/// <summary>
		/// Call this when you need to move the player by animation/timeline. Otherwise the movement might look weird.
		/// </summary>
		/// <param name="isFlag"></param>
		public void MoveStart(bool isFlag)
		{
			parentMoveFollowChild.enabled = !isFlag;
			characterController.enabled = !isFlag;

			characterController.transform.localPosition = Vector3.zero;
		}

		/// <summary>
		/// Move to target location and turn towards target.
		/// </summary>
		/// <param name="moveToTarget"></param>
		/// <param name="turnTowardsTarget"></param>
		/// <returns></returns>
		public async UniTask MoveTo(PlayerMoveToInfo playerMoveToInfo)
		{
			MoveStart(true);
			fsm.StateDictionary.TryGetValue(typeof(PlayerMoveToState), out StateBase stateBase);

			var playerMoveToState = (PlayerMoveToState)stateBase;
			playerMoveToState.SetTarget(playerMoveToInfo);

			fsm.IFSMHandler?.OnBegin(typeof(PlayerMoveToState));

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => playerMoveToState.IsReached, cancellationToken: this.GetCancellationTokenOnDestroy());

			//fsm.IFSMHandler?.OnExit();
			MoveStart(false);
		}

		/// <summary>
		/// Look at target.
		/// </summary>
		/// <param name="lookAtInfo"></param>
		/// <returns></returns>
		public async UniTask LookAt(LookAtInfo lookAtInfo)
		{
			fsm.SetLookAtInfo(lookAtInfo);
			fsm.IFSMHandler?.OnBegin(typeof(PlayerLookAtState));

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => ((PlayerLookAtState)fsm.CurrentState).IsStateEnded, cancellationToken: this.GetCancellationTokenOnDestroy());

			fsm.SetLookAtInfo(null);
		}

		/// <summary>
		/// Main camera will stay looking at target position(from virtual camera position/rotation) even after changing back to default state.
		/// </summary>
		/// <param name="vCam">Should be the virtual camera within the player state</param>
		/// <param name="lookAtInfo"></param>
		/// <returns></returns>
		public async UniTask HandleVCamPersistantLook(CinemachineVirtualCamera vCam, LookAtInfo lookAtInfo)
		{
			var parent = vCam.transform.parent;
			vCam.transform.SetParent(null);

			if (!lookAtInfo.IsInstant)
			{
				rotateBodyCR?.StopCoroutine();

				await UniTask.NextFrame();

				RotateByAnimation(lookAtInfo.LookAt.position);
				await UniTask.WaitUntil(() => rotateBodyCR.IsDone, cancellationToken: this.GetCancellationTokenOnDestroy());
			}

			// Rotate the player's transform to look at target, on the horizontal axis.
			Vector3 lookAtPos = lookAtInfo.LookAt.position;
			lookAtPos.y = 0;

			transform.LookAt(lookAtPos);
			vCam.transform.SetParent(parent);

			FPSController.UpdateTargetPitch(vCam.transform.eulerAngles.x);
		}

		void RotateByAnimation(Vector3 lookAtPosition)
		{
			var cinemachineBrain = StageManager.Instance.CameraHandler.CinemachineBrain;
			float duration = cinemachineBrain.m_DefaultBlend.BlendTime;

			var direction = transform.position.GetNormalizedDirectionTo(lookAtPosition);
			direction.y = 0;

			var endRotation = Quaternion.LookRotation(direction);
			rotateBodyCR = CoroutineHelper.QuaternionLerpWithinSeconds(transform, transform.rotation, endRotation, duration, space: Space.World);
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