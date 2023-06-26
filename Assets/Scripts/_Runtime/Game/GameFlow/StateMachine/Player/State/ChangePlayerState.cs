using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class ChangePlayerState : ActorStateBase
	{
		[SerializeField] string namespaceStr = "Personal.FSM.Character.";
		[SerializeField] Object playerState = null;

		IFSMHandler ifsmHandler;

		public override UniTask OnEnter()
		{
			base.OnEnter();

			PlayerStateMachine playerFSM = StageManager.Instance.PlayerController.FSM;

			playerFSM.SetLookAtTarget(actorStateMachine.transform);
			if (actorStateMachine.ActorController)
			{
				playerFSM.SetLookAtTarget(actorStateMachine.ActorController.Head);
			}

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