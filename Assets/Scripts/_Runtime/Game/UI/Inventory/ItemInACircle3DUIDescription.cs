using System;
using UnityEngine;

using TMPro;
using Helper;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Localization;
using static Personal.Character.Player.PlayerInventory;
using static Personal.Manager.InputManager;

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
			if (playerInventory.InventoryList.Count <= 0) return;

			Vector3 move = InputManager.Instance.GetMotion(MotionType.Move);

			if (move == Vector3.zero) return;
			if (!rotateAroundCR.IsDone) return;

			float angle = yAngleToRotate;
			Action doLast = GetNextAction(false);

			if (move.x < 0 || move.y < 0)
			{
				angle = -yAngleToRotate;
				doLast = GetNextAction(true);
			}

			Vector3 angleRotation = new Vector3(0, angle, 0);
			rotateAroundCR = CoroutineHelper.RotateWithinSeconds(contentTrans, angleRotation, rotateDuration, doLast, false);

			ResetAllInventoryRotations();
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

				titleStr = MasterLocalization.Get(MasterLocalization.TableNameType.NameText, entity.id);
				descriptionStr = MasterLocalization.Get(MasterLocalization.TableNameType.DescriptionText, entity.id);
			}

			titleTMP.text = titleStr;
			descriptionTMP.text = descriptionStr;
		}
	}
}