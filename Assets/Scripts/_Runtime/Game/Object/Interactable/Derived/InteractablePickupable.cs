using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;
using Personal.Item;
using Personal.Manager;
using Personal.Quest;

namespace Personal.InteractiveObject
{
	public class InteractablePickupable : InteractableObject
	{
		[Header("FPS view")]
		[Tooltip("The rotation of the item in the fps view.")]
		[SerializeField] Vector3 fpsRotation = Vector3.zero;

		[Tooltip("The scale of the item in the fps view.")]
		[SerializeField] Vector3 fpsScale = Vector3.one;

		[Header("Inventory view")]
		[Tooltip("The scale of the item in the inventory view.")]
		[SerializeField] Vector3 inventoryRotation = Vector3.zero;

		[Tooltip("The scale of the item in the inventory view.")]
		[SerializeField] Vector3 inventoryScale = Vector3.one;

		public ItemTypeSet ItemTypeSet { get; private set; }
		public SelfRotate SelfRotate { get; private set; }
		public Vector3 FPSRotation { get => fpsRotation; }
		public Vector3 FPSScale { get => fpsScale; }
		public Vector3 InventoryRotation { get => inventoryRotation; }
		public Vector3 InventoryScale { get => inventoryScale; }

		List<QuestTypeSet> questTypeSetList = new();

		protected override void Initialize()
		{
			base.Initialize();

			ItemTypeSet = GetComponentInParent<ItemTypeSet>(true);
			SelfRotate = GetComponentInParent<SelfRotate>(true);

			questTypeSetList = GetComponentsInChildren<QuestTypeSet>().ToList();
		}

		protected override UniTask HandleInteraction()
		{
			HandlePickupable();

			// Update quest info.
			foreach (var questTypeSet in questTypeSetList)
			{
				QuestManager.Instance.TryUpdateData(questTypeSet.QuestType).Forget();
			}

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