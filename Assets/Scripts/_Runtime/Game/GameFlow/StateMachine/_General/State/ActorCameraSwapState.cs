using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Interface;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class ActorCameraSwapState : ActorStateBase
	{
		[SerializeField] Transform iProcessCompleteTrans = null;

		protected PlayerStateMachine playerStateMachine;

		protected CinemachineVirtualCamera cam;
		protected bool isRunning;

		IProcess iProcessComplete;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			cam = GetComponentInChildren<CinemachineVirtualCamera>(true);
			iProcessComplete = iProcessCompleteTrans?.GetComponentInChildren<IProcess>();

			playerStateMachine = StageManager.Instance.PlayerFSM;

			ActivateCamera(true);
			await UniTask.WaitUntil(() => !isRunning);

			actorStateMachine.SetState(null).Forget();
		}

		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();

			// TODO: Input hacking
			if (Input.GetKeyDown(KeyCode.Q) ||
				(iProcessCompleteTrans && iProcessComplete.IsDone()))
			{
				isRunning = false;
			}
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			ActivateCamera(false);
		}

		void ActivateCamera(bool isFlag)
		{
			cam.gameObject.SetActive(isFlag);
			playerStateMachine.FirstPersonController.enabled = !isFlag;
			CursorManager.Instance.SetToMouseCursor(isFlag);

			isRunning = isFlag;
		}
	}
}