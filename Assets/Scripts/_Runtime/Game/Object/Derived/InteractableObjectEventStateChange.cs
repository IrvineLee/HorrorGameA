using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.Object
{
	public class InteractableObjectEventStateChange : InteractableObject
	{
		[SerializeField] protected ActionMapType actionMapType = ActionMapType.Player;

		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;

		protected override void Initialize()
		{
			base.Initialize();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
		}

		protected override async UniTask HandleInteraction(ActorStateMachine actorStateMachine)
		{
			var ifsmHandler = actorStateMachine.GetComponentInChildren<IFSMHandler>();
			await HandleEventStateChange(ifsmHandler);
		}

		/// <summary>
		/// Handle the orderedStateMachine.
		/// </summary>
		/// <returns></returns>
		async UniTask HandleEventStateChange(IFSMHandler ifSMHandler)
		{
			ifSMHandler?.OnBegin();
			InputManager.Instance.EnableActionMap(actionMapType);

			await orderedStateMachine.Initialize(null, interactionAssign);

			InputManager.Instance.SetToDefaultActionMap();
			ifSMHandler?.OnExit();
		}
	}
}

