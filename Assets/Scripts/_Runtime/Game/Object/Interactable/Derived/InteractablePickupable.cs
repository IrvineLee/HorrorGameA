using Cysharp.Threading.Tasks;
using Personal.Item;
using Personal.Manager;
using UnityEngine;

namespace Personal.InteractiveObject
{
	public class InteractablePickupable : InteractableObject
	{
		[Tooltip("The rotation of the item in the fps view.")]
		[SerializeField] Vector3 fpsRotation = Vector3.zero;

		[Tooltip("The scale of the item in the fps view.")]
		[SerializeField] Vector3 fpsScale = Vector3.one;

		public ItemTypeSet ItemTypeSet { get; private set; }
		public Vector3 FPSRotation { get => fpsRotation; }
		public Vector3 FPSScale { get => fpsScale; }

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

