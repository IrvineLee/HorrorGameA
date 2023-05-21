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

		protected CinemachineVirtualCamera virtualCam;
		protected bool isRunning;

		IProcess iProcessComplete;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			iProcessComplete = iProcessCompleteTrans?.GetComponentInChildren<IProcess>();
			if (iProcessComplete.IsCompleted()) return;

			virtualCam = GetComponentInChildren<CinemachineVirtualCamera>(true);

			await ActivateCamera(true);

			iProcessComplete.Begin(true);
			await UniTask.WaitUntil(() => !isRunning);

			// Because this is the final state, put it to null to begin the OnExit function.
			stateMachine.SetState(null).Forget();
		}

		public override void OnUpdate()
		{
			if (InputManager.Instance.IsCancel ||
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
			CursorManager.Instance.SetToMouseCursor(isFlag);

			isRunning = isFlag;
			await UniTask.WaitUntil(() => !StageManager.Instance.CinemachineBrain.IsBlending);
		}
	}
}