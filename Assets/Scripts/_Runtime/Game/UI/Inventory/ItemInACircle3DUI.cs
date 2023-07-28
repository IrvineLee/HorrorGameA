using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.InputProcessing;
using Personal.Manager;
using Personal.Item;
using Personal.System.Handler;
using Personal.InteractiveObject;
using Personal.Character.Player;
using Helper;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class ItemInACircle3DUI : MenuUIBase, IAngleDirection
	{
		[SerializeField] protected Transform contentTrans = null;
		[SerializeField] protected float radius = 5f;
		[SerializeField] protected float rotateDuration = 0.25f;

		protected PlayerInventory playerInventory;

		protected CoroutineRun rotateAroundCR = new CoroutineRun();

		protected float yAngleToRotate;

		public override void InitialSetup()
		{
			playerInventory = StageManager.Instance.PlayerController.Inventory;
		}

		void Update()
		{
			HandleInput();
		}

		/// <summary>
		/// Spawn objects from addressable into a parent. Set objects into UI-based.
		/// </summary>
		/// <param name="itemType"></param>
		/// <param name="inventory"></param>
		/// <returns></returns>
		public async UniTask SpawnObject(ItemType itemType, Inventory inventory)
		{
			GameObject go = PoolManager.Instance.GetSpawnedObject(inventory.PickupableObject.name);
			if (!go) go = await AddressableHelper.Spawn(itemType.GetStringValue(), Vector3.zero);

			go.transform.SetParent(contentTrans);
			go.transform.SetLayerAllChildren((int)LayerType._UI);
			go.transform.localRotation = Quaternion.Euler(inventory.PickupableObject.InventoryRotation);
			go.transform.localScale = inventory.PickupableObject.InventoryScale;

			inventory.SetInteractableObjectUI(go.GetComponentInChildren<InteractablePickupable>());
		}

		/// <summary>
		/// Set objects into a circle and put the active object in front.
		/// </summary>
		public virtual void PutObjectsIntoACircle()
		{
			if (!IsSetIntoACircle()) return;
			if (playerInventory.ActiveObject == null) return;

			// Make sure all rotations are at their default values.
			ResetAllInventoryRotations();

			// Put the active item at the front view.
			int currentIndex = playerInventory.CurrentActiveIndex;
			float eulerRotateToActiveItem = currentIndex * yAngleToRotate;

			// Since it spawns clockwise starting at 6 o'clock, minus it to reach the correct index.
			contentTrans.localEulerAngles = contentTrans.localEulerAngles.With(y: contentTrans.localEulerAngles.y - eulerRotateToActiveItem);

			// Enable the rotation. The reason why you can't use active object is because it might be null.
			// CurrentIndex will always point to the previous selected object.
			playerInventory.InventoryList[currentIndex].PickupableObjectUI.SelfRotate.enabled = true;
		}

		/// <summary>
		/// Handle the movement of items within a circle.
		/// </summary>
		protected virtual void HandleInput()
		{
			if (InputManager.Instance.Move == Vector3.zero) return;
			if (!rotateAroundCR.IsDone) return;

			float angle = yAngleToRotate;
			if (InputManager.Instance.Move.x < 0)
			{
				angle = -yAngleToRotate;
			}

			Vector3 angleRotation = new Vector3(0, angle, 0);
			rotateAroundCR = CoroutineHelper.RotateWithinSeconds(contentTrans, angleRotation, rotateDuration, default, false);
		}

		/// <summary>
		/// Reset all inventory's rotations.
		/// </summary>
		protected void ResetAllInventoryRotations()
		{
			foreach (Inventory inventory in playerInventory.InventoryList)
			{
				inventory.PickupableObjectUI.transform.localRotation = Quaternion.Euler(inventory.PickupableObject.InventoryRotation);
				inventory.PickupableObjectUI.SelfRotate.enabled = false;
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

				//child.transform.SetParent(contentTrans);
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