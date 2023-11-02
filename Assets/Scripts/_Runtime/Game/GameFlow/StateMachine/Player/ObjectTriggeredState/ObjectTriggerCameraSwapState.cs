using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Interface;
using Personal.Manager;
using static Personal.Manager.InputManager;

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
			await UniTask.WaitUntil(() => !(bool)isRunning, cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		public override void OnUpdate()
		{
			if (isRunning == null) return;

			if (iProcessTrans != null &&
				((InputManager.Instance.GetButtonPush(ButtonPush.Cancel) && iProcess.IsExit()) ||
				(iProcess.IsCompleted() || iProcess.IsFailed())))
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

		public override async UniTask Standby()
		{
			await WaitCameraBlend();
		}

		async UniTask ActivateCamera(bool isFlag)
		{
			virtualCam.gameObject.SetActive(isFlag);
			CursorManager.Instance.TrySetToMouseCursorForMouseControl(isFlag, true);

			isRunning = isFlag;
			await WaitCameraBlend();
		}

		async UniTask WaitCameraBlend()
		{
			await UniTask.WaitUntil(() => !StageManager.Instance.CameraHandler.CinemachineBrain.IsBlending, cancellationToken: this.GetCancellationTokenOnDestroy());
		}
	}
}