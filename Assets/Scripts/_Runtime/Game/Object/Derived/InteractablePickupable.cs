using Cysharp.Threading.Tasks;
using Personal.Item;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractablePickupable : InteractableObject
	{
		public ItemTypeSet ItemTypeSet { get; private set; }

		protected override void Initialize()
		{
			base.Initialize();

			ItemTypeSet = GetComponentInParent<ItemTypeSet>();
		}

		protected override UniTask HandleInteraction()
		{
			HandlePickupable();
			return UniTask.CompletedTask;
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

