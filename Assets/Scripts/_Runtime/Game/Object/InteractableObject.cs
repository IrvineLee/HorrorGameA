using System;
using UnityEngine;

using Personal.GameState;
using Personal.FSM;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class InteractableObject : GameInitialize
	{
		public enum InteractType
		{
			Pickupable = 0,
			StateChange,
		}

		[SerializeField] protected InteractType interactType = InteractType.Pickupable;

		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;

		protected override async UniTask Awake()
		{
			await base.Awake();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
		}

		public virtual async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast) { await UniTask.Delay(0); }
	}
}

