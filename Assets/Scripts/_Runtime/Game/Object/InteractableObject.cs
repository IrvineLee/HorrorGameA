using System;
using UnityEngine;

using Personal.GameState;
using Personal.FSM;
using Personal.InputProcessing;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.Item;

namespace Personal.Object
{
	public abstract class InteractableObject : GameInitialize
	{
		[SerializeField] protected InteractType interactType = InteractType.Pickupable;
		[SerializeField] private Transform parentTrans = null;

		[ShowIf("interactType", InteractType.Event_StateChange)]
		[SerializeField] protected ActionMapType actionMapType = ActionMapType.Player;

		[ShowIf("interactType", InteractType.UseActiveItem)]
		[SerializeField] protected ItemType itemTypeCompare = ItemType.Item_1;

		[ShowIf("interactType", InteractType.UseActiveItem)]
		[SerializeField] protected Transform placeAt = null;

		public InteractType InteractType { get => interactType; }
		public Transform ParentTrans { get => parentTrans; }

		/// <summary>
		/// Not every interactableObject will be an item.
		/// </summary>
		public ItemTypeSet ItemTypeSet { get; private set; }

		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;

		protected Collider currentCollider;
		protected MeshRenderer meshRenderer;

		protected override async UniTask Awake()
		{
			await base.Awake();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();

			currentCollider = GetComponentInChildren<Collider>();
			meshRenderer = GetComponentInChildren<MeshRenderer>();

			ItemTypeSet = GetComponentInParent<ItemTypeSet>();
		}

		public virtual async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast) { await UniTask.Delay(0); }
	}
}

