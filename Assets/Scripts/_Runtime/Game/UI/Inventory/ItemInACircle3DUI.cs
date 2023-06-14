using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.InputProcessing;
using Personal.Manager;
using Personal.Item;
using Personal.System.Handler;
using Personal.Object;
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
		protected UIInputController uIInputController;

		protected CoroutineRun rotateAroundCR = new CoroutineRun();

		protected float yAngleToRotate;

		public override void InitialSetup()
		{
			uIInputController = InputManager.Instance.UIInputController;
			playerInventory = StageManager.Instance.PlayerController.Inventory;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

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
			if (!go) go = await AddressableHelper.Spawn(itemType.GetStringValue(), Vector3.zero, contentTrans);

			go.transform.SetLayerAllChildren((int)LayerType._UI);
			go.transform.localScale = Vector3.one;

			inventory.SetInteractableObjectUI(go.GetComponentInChildren<InteractablePickupable>());
		}

		/// <summary>
		/// Set objects into a circle and put the active object in front.
		/// </summary>
		public virtual void Setup()
		{
			if (!IsSetIntoACircle()) return;

			// Put the active item at the front view.
			int currentIndex = StageManager.Instance.PlayerController.Inventory.CurrentActiveIndex;
			float eulerRotateToActiveItem = currentIndex * yAngleToRotate;

			contentTrans.localEulerAngles = contentTrans.localEulerAngles.With(y: contentTrans.localEulerAngles.y + eulerRotateToActiveItem);
		}

		/// <summary>
		/// Handle the movement of items within a circle.
		/// </summary>
		protected virtual void HandleInput()
		{
			if (uIInputController.Move == Vector2.zero) return;
			if (!rotateAroundCR.IsDone) return;

			float angle = yAngleToRotate;
			if (uIInputController.Move.x < 0)
			{
				angle = -yAngleToRotate;
			}

			Vector3 angleRotation = new Vector3(0, angle, 0);
			rotateAroundCR = CoroutineHelper.RotateWithinSeconds(contentTrans, angleRotation, rotateDuration, default, false);
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
				Vector2 direction = directionList[i] * radius;

				child.position = contentTrans.position;
				child.localPosition = child.localPosition.With(x: direction.x, z: direction.y);
			}

			// Since the first item spawned would be straight ahead, rotate it 180 degrees so it faces the front.
			contentTrans.localRotation = Quaternion.identity;
			contentTrans.localEulerAngles = Vector3.zero.With(y: 180);

			Quaternion quaternion = Quaternion.Euler(contentTrans.localEulerAngles);
			contentTrans.localRotation = quaternion;

			yAngleToRotate = 360 / childCount;
			return true;
		}
	}
}