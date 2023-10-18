using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Definition;
using Personal.GameState;
using Helper;

namespace Personal.InteractiveObject
{
	public abstract class InteractableObject : GameInitialize
	{
		[SerializeField] Transform parentTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		[SerializeField] protected bool isInteractable = true;

		public Transform ParentTrans { get => parentTrans; }
		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine InitiatorStateMachine { get; protected set; }

		protected Collider currentCollider;
		protected MeshRenderer meshRenderer;

		protected OutlinableFadeInOut outlinableFadeInOut;

		protected override void Initialize()
		{
			currentCollider = GetComponentInChildren<Collider>();
			meshRenderer = GetComponentInChildren<MeshRenderer>();

			outlinableFadeInOut = GetComponentInChildren<OutlinableFadeInOut>(true);
		}

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			if (!isInteractable) await UniTask.CompletedTask;

			InitiatorStateMachine = initiatorStateMachine;

			await HandleInteraction();
			doLast?.Invoke();
		}

		public void ShowOutline(bool isFlag)
		{
			outlinableFadeInOut?.StartFade(isFlag);
		}

		protected virtual async UniTask HandleInteraction() { await UniTask.CompletedTask; }
	}
}

