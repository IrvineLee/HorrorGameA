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
		protected bool? isRunning;

		IProcess iProcess;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			isRunning = null;

			iProcess = iProcessTrans?.GetComponentInChildren<IProcess>();
			virtualCam = GetComponentInChildren<CinemachineVirtualCamera>(true);

			await ActivateCamera(true);

			iProcess.Begin(true);
			isRunning = true;
			await UniTask.WaitUntil(() => !(bool)isRunning);
		}

		public override void OnUpdate()
		{
			if (isRunning == null) return;

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

		public override async UniTask CheckComparisonDo()
		{
			await WaitCameraBlend();
		}

		async UniTask ActivateCamera(bool isFlag)
		{
			virtualCam.gameObject.SetActive(isFlag);
			CursorManager.Instance.TrySetToMouseCursorForMouseControl(true);

			isRunning = isFlag;
			await WaitCameraBlend();
		}

		async UniTask WaitCameraBlend()
		{
			await UniTask.WaitUntil(() => StageManager.Instance && !StageManager.Instance.CameraHandler.CinemachineBrain.IsBlending);
		}
	}
}