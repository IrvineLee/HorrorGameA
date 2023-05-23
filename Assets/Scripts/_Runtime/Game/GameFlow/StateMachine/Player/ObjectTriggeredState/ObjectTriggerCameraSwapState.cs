using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Interface;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class ObjectTriggerCameraSwapState : StateBase
	{
		[SerializeField] Transform iProcessTrans = null;

		protected CinemachineVirtualCamera virtualCam;
		protected bool isRunning;

		IProcess iProcess;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			iProcess = iProcessTrans?.GetComponentInChildren<IProcess>();
			if (iProcess.IsCompleted()) return;

			virtualCam = GetComponentInChildren<CinemachineVirtualCamera>(true);

			await ActivateCamera(true);

			iProcess.Begin(true);
			await UniTask.WaitUntil(() => !isRunning);
		}

		public override void OnUpdate()
		{
			if (InputManager.Instance.IsCancel ||
				(iProcessTrans != null && (iProcess.IsCompleted() || iProcess.IsFailed())))
			{
				isRunning = false;
			}
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			iProcess.Begin(false);
			await ActivateCamera(false);
		}

		async UniTask ActivateCamera(bool isFlag)
		{
			virtualCam.gameObject.SetActive(isFlag);
			CursorManager.Instance.SetToMouseCursor(isFlag);

			isRunning = isFlag;
			await UniTask.WaitUntil(() => StageManager.Instance && !StageManager.Instance.CinemachineBrain.IsBlending);
		}
	}
}