using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.FSM.Character
{
	public class SetPlayerMoveState : StateBase
	{
		[SerializeField] Transform moveToTarget = null;
		[SerializeField] Transform turnTowardsTarget = null;
		[SerializeField] float turnTowardsSpeed = 1f;

		[Tooltip("The duration for the camera to lerp to rotation 0")]
		[SerializeField] float cameraResetRotationDuration = 1f;

		PlayerController playerController;
		StateBase previousState;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playerController = StageManager.Instance.PlayerController;
			previousState = playerController.FSM.CurrentState;

			var cameraGO = playerController.FPSController.CinemachineCameraGO;

			Action<float> callbackMethod = (result) =>
			{
				//result -= 180f;
				////if (result > 90) result -= 270;

				////float x = Mathf.Clamp(result, -90f, 90f);

				//Debug.Break();
				//Debug.Log(result);
				//cameraGO.transform.localRotation = Quaternion.Euler(new Vector3(result, 0, 0));
				cameraGO.transform.localRotation = Quaternion.Euler(cameraGO.transform.localRotation.eulerAngles.With(x: result));
			};
			Action doLast = () => playerController.FPSController.UpdateTargetPitch(0);

			CoroutineHelper.LerpWithinSeconds(cameraGO.transform.localRotation.eulerAngles.x, 0, cameraResetRotationDuration, callbackMethod, doLast);

			PlayerMoveToInfo playerMoveToInfo = new PlayerMoveToInfo(moveToTarget, turnTowardsTarget, turnTowardsSpeed);
			await playerController.MoveTo(playerMoveToInfo);
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
			playerController.FSM.IFSMHandler?.OnBegin(previousState.GetType());
		}
	}
}