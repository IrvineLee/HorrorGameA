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

		float radius = ConstantFixed.PLAYER_LOOK_SPHERECAST_RADIUS;
		float length = ConstantFixed.PLAYER_LOOK_SPHERECAST_LENGTH;

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public override UniTask OnEnter()
		{
			base.OnEnter();
			playerFSM = (PlayerStateMachine)stateMachine;

			cam = StageManager.Instance.MainCamera;
			return UniTask.CompletedTask;
		}

		public override void OnUpdate()
		{
			RaycastHit hit;

			Vector3 startPos = cam.transform.position;
			Vector3 endPos = startPos + cam.transform.forward * length;

			Debug.DrawLine(startPos, endPos, Color.green);
			if (Physics.SphereCast(startPos, radius, cam.transform.forward, out hit, length, 1 << (int)LayerType._Interactable))
			{
				HandleInteractable(hit);
				return;
			}

			CursorManager.Instance.SetToDefaultCrosshair();
		}

		protected virtual void HandleInteractable(RaycastHit hit) { }
	}
}