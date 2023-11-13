using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Item;
using Personal.Manager;
using Personal.Quest;

namespace Personal.InteractiveObject
{
	public class InteractablePickupable : InteractableObject
	{
		[Header("Item Setting")]
		[SerializeField] ItemType itemType = ItemType._10000_Item_1;

		[SerializeField] Transform fpsPrefab = null;
		[SerializeField] SelfRotate uiPrefab = null;

		public ItemType ItemType { get => itemType; }

		public Transform FPSPrefab { get => fpsPrefab; }
		public SelfRotate UIPrefab { get => uiPrefab; }

		List<QuestTypeSet> questTypeSetList = new();

		protected override void Initialize()
		{
			base.Initialize();

			questTypeSetList = GetComponentsInChildren<QuestTypeSet>().ToList();

			if (itemType == default) return;
		}

		protected override UniTask HandleInteraction()
		{
			HandlePickupable();

			// Update quest info.
			foreach (var questTypeSet in questTypeSetList)
			{
				QuestManager.Instance.TryUpdateData(questTypeSet.QuestType);
			}

			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Add item into inventory.
		/// </summary>
		void HandlePickupable()
		{
			StageManager.Instance.PlayerController.Inventory.AddItem(this);

			colliderTrans.gameObject.SetActive(false);
			meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			enabled = false;
		}
	}
}