using System;
using UnityEngine;

using Personal.Manager;
using Personal.Character.Player;
using Helper;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class ItemInACircle3DUI : MenuUI, IAngleDirection
	{
		[SerializeField] protected Transform contentTrans = null;
		[SerializeField] protected float radius = 5f;
		[SerializeField] protected float rotateDuration = 0.25f;

		protected PlayerInventory playerInventory;
		protected float yAngleToRotate;

		protected CoroutineRun rotateAroundCR = new CoroutineRun();

		public override void InitialSetup()
		{
			playerInventory = StageManager.Instance.PlayerController.Inventory;
		}

		/// <summary>
		/// Set objects into UI-based.
		/// </summary>
		/// <param name="itemType"></param>
		/// <param name="inventory"></param>
		/// <returns></returns>
		public void Init(SelfRotate inventoryUI)
		{
			Quaternion rotation = inventoryUI.transform.localRotation;
			Vector3 scale = inventoryUI.transform.localScale;

			inventoryUI.transform.SetParent(contentTrans);
			inventoryUI.transform.localRotation = rotation;
			inventoryUI.transform.localScale = scale;
		}

		/// <summary>
		/// Set objects into a circle and put the active object in front.
		/// </summary>
		public virtual void PutObjectsIntoACircle()
		{
			if (playerInventory.InventoryList.Count <= 0) return;
			if (!IsSetIntoACircle()) return;

			// Make sure all rotations are at their default values.
			ResetAllInventoryRotations();

			// Put the active item at the front view.
			int currentIndex = playerInventory.CurrentActiveIndex;
			float eulerRotateToActiveItem = currentIndex * yAngleToRotate;

			// Since it spawns clockwise starting at 6 o'clock, minus it to reach the correct index.
			contentTrans.localEulerAngles = contentTrans.localEulerAngles.With(y: contentTrans.localEulerAngles.y - eulerRotateToActiveItem);

			// Enable the rotation. The reason why you can't use active object is because it might be null.
			// CurrentIndex will always point to the previous selected object.
			playerInventory.InventoryList[currentIndex].PickupableObjectRotateUI.enabled = true;
		}

		public void HandleInput(bool isNext)
		{
			if (playerInventory.InventoryList.Count <= 0) return;
			if (!rotateAroundCR.IsDone) return;

			float angle = isNext ? -yAngleToRotate : yAngleToRotate;
			Action doLast = ReachedAction(isNext);

			Vector3 angleRotation = new Vector3(0, angle, 0);
			rotateAroundCR = CoroutineHelper.RotateWithinSeconds(contentTrans, angleRotation, rotateDuration, doLast, false);

			ResetAllInventoryRotations();
		}

		/// <summary>
		/// The action when it reached the target.
		/// </summary>
		/// <param name="isNextItem"></param>
		/// <returns></returns>
		protected virtual Action ReachedAction(bool isNextItem) { return default; }

		/// <summary>
		/// Reset all inventory's rotations.
		/// </summary>
		protected void ResetAllInventoryRotations()
		{
			foreach (PlayerInventory.Item item in playerInventory.InventoryList)
			{
				item.ResetPickupableObjectUI();
			}
		}

		/// <summary>
		/// The calculation of putting objects into a circle.
		/// </summary>
		/// <returns></returns>
		bool IsSetIntoACircle()
		{
			// Put all the children into a circle.
			int childCount = contentTrans.childCount;
			if (childCount <= 0) return false;

			var directionList = IAngleDirection.GetDirectionListFor360Degrees(childCount, 0);

			for (int i = 0; i < childCount; i++)
			{
				Transform child = contentTrans.GetChild(i);
				Vector2 direction = -directionList[i] * radius; // Negative direction so it to spawns at 6 o'clock rather than 12 o'clock.

				child.position = contentTrans.position;
				child.localPosition = child.localPosition.With(x: direction.x, y: 0, z: direction.y);
			}

			contentTrans.localRotation = Quaternion.identity;

			Quaternion quaternion = Quaternion.Euler(contentTrans.localEulerAngles);
			contentTrans.localRotation = quaternion;

			yAngleToRotate = 360 / childCount;
			return true;
		}
	}
}