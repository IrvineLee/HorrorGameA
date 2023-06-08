using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.FSM;

namespace Personal.Object
{
	public abstract class InteractableObject : GameInitialize
	{
		[SerializeField] private Transform parentTrans = null;

		public Transform ParentTrans { get => parentTrans; }

		protected Collider currentCollider;
		protected MeshRenderer meshRenderer;

		protected override async UniTask Awake()
		{
			await base.Awake();

			currentCollider = GetComponentInChildren<Collider>();
			meshRenderer = GetComponentInChildren<MeshRenderer>();
		}

		public virtual async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast) { await UniTask.Delay(0); }
	}
}

