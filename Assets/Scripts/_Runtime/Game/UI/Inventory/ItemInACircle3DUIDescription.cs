using System;
using UnityEngine;

using TMPro;
using Personal.Object;
using Helper;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class ItemInACircle3DUIDescription : ItemInACircle3DUI
	{
		[SerializeField] TextMeshProUGUI titleTMP = null;
		[SerializeField] TextMeshProUGUI descriptionTMP = null;

		public override void Setup()
		{
			base.Setup();

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
			SetInfoToText(currentInventory.InteractableObject);
		}

		void SetInfoToText(InteractableObject interactableObject)
		{
			string titleStr = "";
			string descriptionStr = "";

			if (interactableObject)
			{
				var entity = interactableObject.ItemTypeSet.Entity;

				titleStr = entity.name;
				descriptionStr = entity.description;
			}

			titleTMP.text = titleStr;
			descriptionTMP.text = descriptionStr;
		}
	}
}