using System;
using UnityEngine;

using Personal.GameState;
using Personal.FSM;
using Personal.InputProcessing;
using Cysharp.Threading.Tasks;

namespace Personal.Object
{
	public abstract class InteractableObject : GameInitialize
	{
		public enum InteractType
		{
			Pickupable = 0,
			StateChange,
		}

		[SerializeField] protected InteractType interactType = InteractType.Pickupable;
		[SerializeField] protected ActionMapType actionMapType = ActionMapType.Player;

		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;
		protected Collider currentCollider;

		protected override async UniTask Awake()
		{
			await base.Awake();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
			currentCollider = GetComponentInChildren<Collider>();
		}

		public virtual async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast) { await UniTask.Delay(0); }
	}
}

