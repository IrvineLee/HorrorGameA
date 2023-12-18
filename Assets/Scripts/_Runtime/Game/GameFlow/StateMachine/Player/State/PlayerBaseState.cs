using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Constant;
using Personal.Manager;
using Personal.System.Handler;

namespace Personal.FSM.Character
{
	public abstract class PlayerBaseState : ActorStateBase
	{
		protected PlayerStateMachine playerFSM;

		protected Camera cam;
		protected IFSMHandler ifsmHandler;

		float radius = ConstantFixed.PLAYER_LOOK_SPHERECAST_RADIUS;
		float length = ConstantFixed.PLAYER_LOOK_SPHERECAST_LENGTH;

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			playerFSM = (PlayerStateMachine)stateMachine;

			cam = StageManager.Instance.CameraHandler.MainCamera;
			ifsmHandler = playerFSM.IFSMHandler;
		}

		public override void OnUpdate()
		{
			RaycastHit hit;

			Vector3 startPos = cam.transform.position;
			Vector3 endPos = startPos + cam.transform.forward * length;

			Debug.DrawLine(startPos, endPos, Color.green);
			if (Physics.SphereCast(startPos, radius, cam.transform.forward, out hit, length, 1 << (int)LayerType._Interactable))
			{
				HandleOnInteractable(hit);
				return;
			}

			CursorManager.Instance.SetCenterCrosshairToDefault();
			HandleOffInteractable();
		}

		protected virtual void HandleOnInteractable(RaycastHit hit) { }
		protected virtual void HandleOffInteractable() { }
	}
}