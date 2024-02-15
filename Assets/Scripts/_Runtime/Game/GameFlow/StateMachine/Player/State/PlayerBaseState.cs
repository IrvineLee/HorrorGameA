using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public abstract class PlayerBaseState : ActorStateBase
	{
		protected PlayerStateMachine playerFSM;

		protected Camera cam;
		protected IFSMHandler ifsmHandler;

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

		protected virtual void HandleOnInteractable(RaycastHit hit) { }
		protected virtual void HandleOffInteractable() { }
	}
}