using System;
using UnityEngine;

using TMPro;
using Personal.InteractiveObject;
using Personal.Localization;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class ItemInACircle3DUIDescription : ItemInACircle3DUI
	{
		[SerializeField] TextMeshProUGUI titleTMP = null;
		[SerializeField] TextMeshProUGUI descriptionTMP = null;

		public override void PutObjectsIntoACircle()
		{
			base.PutObjectsIntoACircle();

			UpdateText();
		}

		/// <summary>
		/// Checks whether the input is moving to the right or left item.
		/// </summary>
		/// <param name="isNextItem"></param>
		/// <returns></returns>
		protected override Action ReachedAction(bool isNextItem)
		{
			Action action = () =>
			{
				playerInventory.NextItem(isNextItem);
				UpdateText();

				var newActiveObject = playerInventory.InventoryList[playerInventory.CurrentActiveIndex];
				newActiveObject.PO_UI_SelfRotate.enabled = true;
			};

			return action;
		}

		void UpdateText()
		{
			// Update the title and description text.
			if (playerInventory.InventoryList.Count <= 0)
			{
				SetInfoToText(null);
				return;
			}

			Inventory currentInventory = playerInventory.InventoryList[playerInventory.CurrentActiveIndex];
			SetInfoToText(currentInventory.PickupableObject);
		}

		void SetInfoToText(InteractablePickupable interactablePickupable)
		{
			string titleStr = "";
			string descriptionStr = "";

			if (interactablePickupable)
			{
				var entity = interactablePickupable.Entity;

				titleStr = MasterLocalization.Get(MasterLocalization.TableNameType.NameText, entity.id);
				descriptionStr = MasterLocalization.Get(MasterLocalization.TableNameType.DescriptionText, entity.id);
			}

			titleTMP.text = titleStr;
			descriptionTMP.text = descriptionStr;
		}
	}
}