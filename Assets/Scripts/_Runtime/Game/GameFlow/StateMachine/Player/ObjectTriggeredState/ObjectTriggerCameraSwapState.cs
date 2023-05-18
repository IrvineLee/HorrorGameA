using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Interface;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class ObjectTriggerCameraSwapState : StateBase
	{
		[SerializeField] Transform iProcessCompleteTrans = null;

		protected PlayerStateMachine playerFSM;

		protected CinemachineVirtualCamera virtualCam;
		protected bool isRunning;

		IProcess iProcessComplete;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			iProcessComplete = iProcessCompleteTrans?.GetComponentInChildren<IProcess>();
			if (iProcessComplete.IsCompleted()) return;

			virtualCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
			playerFSM = StageManager.Instance.PlayerFSM;

			await ActivateCamera(true);

			iProcessComplete.Begin(true);
			await UniTask.WaitUntil(() => !isRunning);

			stateMachine.SetState(null).Forget();
		}

		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();

			// TODO: Input hacking
			if (Input.GetKeyDown(KeyCode.Q) ||
				(iProcessCompleteTrans != null && iProcessComplete.IsCompleted()))
			{
				isRunning = false;
			}
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			iProcessComplete.Begin(false);
			await ActivateCamera(false);
		}

		async UniTask ActivateCamera(bool isFlag)
		{
			virtualCam.gameObject.SetActive(isFlag);
			playerFSM.FirstPersonController.enabled = !isFlag;
			CursorManager.Instance.SetToMouseCursor(isFlag);

			isRunning = isFlag;
			await UniTask.WaitUntil(() => !StageManager.Instance.CinemachineBrain.IsBlending);
		}
	}
}