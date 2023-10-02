using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Definition;
using Helper;

namespace Personal.InteractiveObject
{
	public abstract class InteractableObject : MonoBehaviour
	{
		[SerializeField] Transform parentTrans = null;
		[SerializeField] CursorDefinition.CrosshairType interactCrosshairType = CursorDefinition.CrosshairType.FPS;

		public Transform ParentTrans { get => parentTrans; }
		public CursorDefinition.CrosshairType InteractCrosshairType { get => interactCrosshairType; }
		public ActorStateMachine InitiatorStateMachine { get; private set; }

		protected Collider currentCollider;
		protected MeshRenderer meshRenderer;

		protected OutlinableFadeInOut outlinableFadeInOut;

		protected virtual void Awake()
		{
			currentCollider = GetComponentInChildren<Collider>();
			meshRenderer = GetComponentInChildren<MeshRenderer>();

			outlinableFadeInOut = GetComponentInChildren<OutlinableFadeInOut>(true);
		}

		protected virtual async UniTask HandleInteraction() { await UniTask.CompletedTask; }

		public async UniTask HandleInteraction(ActorStateMachine initiatorStateMachine, Action doLast = default)
		{
			InitiatorStateMachine = initiatorStateMachine;

			await HandleInteraction();
			doLast?.Invoke();
		}

		public void ShowOutline(bool isFlag)
		{
			outlinableFadeInOut?.StartFade(isFlag);
		}
	}
}

