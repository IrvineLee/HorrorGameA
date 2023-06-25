using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Personal.FSM.Character
{
	public class PlayerChangeState : ActorStateBase
	{
		[SerializeField] string namespaceStr = "Personal.FSM.Character.";
		[SerializeField] Object playerState = null;

		IFSMHandler ifsmHandler;

		public override UniTask OnEnter()
		{
			base.OnEnter();

			PlayerStateMachine playerFSM = (PlayerStateMachine)stateMachine.InitiatorStateMachine;
			playerFSM.SetLookAtTarget(transform);

			ifsmHandler = playerFSM.GetComponentInChildren<IFSMHandler>();

			Type type = Type.GetType(namespaceStr + playerState.name);
			ifsmHandler?.OnBegin(type);

			return UniTask.CompletedTask;
		}

		public override UniTask CheckComparisonDo()
		{
			ifsmHandler?.OnExit();
			return UniTask.CompletedTask;
		}
	}
}