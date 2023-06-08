﻿using System;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Item;
using Personal.Manager;

namespace Personal.Object
{
	public class InteractablePickupable : InteractableObject
	{
		public ItemTypeSet ItemTypeSet { get; private set; }

		protected override async UniTask Awake()
		{
			await base.Awake();

			ItemTypeSet = GetComponentInParent<ItemTypeSet>();
		}

		public override async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast = default)
		{
			await base.HandleInteraction(stateMachineBase, doLast);
			HandlePickupable();

			doLast?.Invoke();
		}

		/// <summary>
		/// Add item into inventory.
		/// </summary>
		void HandlePickupable()
		{
			StageManager.Instance.PlayerController.Inventory.AddItem(this);

			currentCollider.enabled = false;
			meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			enabled = false;
		}
	}
}

