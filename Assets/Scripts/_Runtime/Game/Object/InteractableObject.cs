using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.FSM;
using Personal.Definition;

namespace Personal.Object
{
	public abstract class InteractableObject : GameInitialize
	{
		[SerializeField] Transform parentTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		public Transform ParentTrans { get => parentTrans; }
		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine ActorStateMachine { get; private set; }

		protected Collider currentCollider;
		protected MeshRenderer meshRenderer;

		protected override void Initialize()
		{
			currentCollider = GetComponentInChildren<Collider>();
			meshRenderer = GetComponentInChildren<MeshRenderer>();
		}

		public virtual async UniTask HandleInteraction(ActorStateMachine actorStateMachine, Action doLast)
		{
			ActorStateMachine = actorStateMachine;
			await HandleInteraction(actorStateMachine);
		}

		protected virtual async UniTask HandleInteraction(ActorStateMachine actorStateMachine) { await UniTask.CompletedTask; }
	}
}

