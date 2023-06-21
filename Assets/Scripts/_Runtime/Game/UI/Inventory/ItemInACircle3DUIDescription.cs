using System;
using UnityEngine;

using TMPro;
using Personal.Manager;
using Personal.Object;
using Helper;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class ItemInACircle3DUIDescription : ItemInACircle3DUI
	{
		[SerializeField] TextMeshProUGUI titleTMP = null;
		[SerializeField] TextMeshProUGUI descriptionTMP = null;

		SetFontAsset setFontAssetTitle;
		SetFontAsset setFontAssetDescription;

		protected override void PreInitialize()
		{
			setFontAssetTitle = titleTMP.GetComponentInChildren<SetFontAsset>();
			setFontAssetDescription = descriptionTMP.GetComponentInChildren<SetFontAsset>();
		}

		public override void PutObjectsIntoACircle()
		{
			base.PutObjectsIntoACircle();

			UpdateText();
		}

		protected override void HandleInput()
		{
			if (uIInputController.Move == Vector2.zero) return;
			if (!rotateAroundCR.IsDone) return;

			float angle = yAngleToRotate;
			Action doLast = GetNextAction(true);

			if (uIInputController.Move.x < 0)
			{
				angle = -yAngleToRotate;
				doLast = GetNextAction(false);
			}

			Vector3 angleRotation = new Vector3(0, angle, 0);
			rotateAroundCR = CoroutineHelper.RotateWithinSeconds(contentTrans, angleRotation, rotateDuration, doLast, false);
		}

		/// <summary>
		/// Checks whether the input is moving to the right or left item.
		/// </summary>
		/// <param name="isNextItem"></param>
		/// <returns></returns>
		Action GetNextAction(bool isNextItem)
		{
			Action action = () =>
			{
				playerInventory.NextItem(isNextItem);
				UpdateText();
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
				setFontAssetTitle.HandleChange();
				setFontAssetDescription.HandleChange();

				var entity = interactablePickupable.ItemTypeSet.Entity;

				titleStr = entity.name;

				string description = MasterDataManager.Instance.Localization.LocalizedData().Item.Get(entity.itemType).description;
				descriptionStr = description;
			}

			titleTMP.text = titleStr;
			descriptionTMP.text = descriptionStr;
		}
	}
}