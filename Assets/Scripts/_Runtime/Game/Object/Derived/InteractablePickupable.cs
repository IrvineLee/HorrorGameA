using System;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Item;
using Personal.Manager;

namespace Personal.Object
{
	public class InteractablePickupable : InteractableObject
	{
		public ItemTypeSet ItemTypeSet { get; private set; }

		protected override void Initialize()
		{
			base.Initialize();

			ItemTypeSet = GetComponentInParent<ItemTypeSet>();
		}

		protected override async UniTask HandleInteraction(ActorStateMachine actorStateMachine)
		{
			await base.HandleInteraction(actorStateMachine);
			HandlePickupable();
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

