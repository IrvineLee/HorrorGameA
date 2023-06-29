using System;
using UnityEngine;

using TMPro;
using Personal.Manager;
using Personal.InteractiveObject;
using Helper;
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

		protected override void HandleInput()
		{
			if (uIInputController.Move == Vector2.zero) return;
			if (!rotateAroundCR.IsDone) return;

			float angle = yAngleToRotate;
			Action doLast = GetNextAction(false);

			if (uIInputController.Move.x < 0)
			{
				angle = -yAngleToRotate;
				doLast = GetNextAction(true);
			}

			Vector3 angleRotation = new Vector3(0, angle, 0);
			rotateAroundCR = CoroutineHelper.RotateWithinSeconds(contentTrans, angleRotation, rotateDuration, doLast, false);

			// Rotate the active pickupable.
			ResetActiveRotation();
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

				var newActiveObject = playerInventory.InventoryList[playerInventory.CurrentActiveIndex];
				newActiveObject.PickupableObjectUI.SelfRotate.enabled = true;
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
				var entity = interactablePickupable.ItemTypeSet.Entity;

				titleStr = MasterDataManager.Instance.Localization.Get(entity.key).NameText;

				string description = MasterDataManager.Instance.Localization.Get(entity.key).DescriptionText;
				descriptionStr = description;
			}

			titleTMP.text = titleStr;
			descriptionTMP.text = descriptionStr;
		}
	}
}