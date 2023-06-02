using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.Object;
using Personal.GameState;
using Personal.Manager;
using Personal.InputProcessing;
using Personal.Item;
using Helper;
using static Personal.Definition.InputReaderDefinition;

namespace Personal.Character.Player
{
	public class PlayerInventory : GameInitialize
	{
		[SerializeField] Transform FPSHoldItemInHandView = null;
		[SerializeField] float autoHideItemDuration = 10f;

		[SerializeField]
		[ReadOnly]
		InteractableObject activeObject = null;

		[SerializeField]
		[ReadOnly]
		List<InteractableObject> interactableObjectList = new();

		public InteractableObject ActiveObject { get => activeObject; }
		public List<InteractableObject> InteractableObjectList { get => interactableObjectList; }

		public int CurrentActiveIndex { get; private set; }

		Vector3 initialPosition = new Vector3(0, -0.25f, 0);
		Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f);

		CoroutineRun comeIntoViewCR = new CoroutineRun();
		CoroutineRun autoHideItemCR = new CoroutineRun();

		InputControllerInfo inputControllerInfo;

		protected async override UniTask Awake()
		{
			await base.Awake();

			inputControllerInfo = InputManager.Instance.GetInputControllerInfo(ActionMapType.Player);
			inputControllerInfo.OnEnableEvent += FPS_ShowItem;
		}

		/// <summary>
		/// Show or hide the active object.
		/// </summary>
		/// <param name="isFlag"></param>
		public void FPS_ShowItem(bool isFlag)
		{
			if (!activeObject) return;

			if (isFlag)
			{
				AnimateActiveItem(Vector3.zero);

				autoHideItemCR?.StopCoroutine();
				autoHideItemCR = CoroutineHelper.WaitFor(autoHideItemDuration, () => FPS_ShowItem(false));
			}
			else
			{
				AnimateActiveItem(initialPosition);
			}
		}

		/// <summary>
		/// Use/interact/place item on someone or something.
		/// </summary>
		public void UseActiveItem()
		{
			activeObject = null;
			interactableObjectList.RemoveAt(CurrentActiveIndex);
		}

		/// <summary>
		/// Add item to inventory.
		/// </summary>
		/// <param name="interactableObject"></param>
		public void AddItem(InteractableObject interactableObject)
		{
			activeObject?.gameObject.SetActive(false);
			activeObject = interactableObject;

			interactableObjectList.Add(interactableObject);
			CurrentActiveIndex = interactableObjectList.Count - 1;

			// Add item to inventory ui.
			ItemType itemType = activeObject.ItemTypeSet.ItemType;
			UIManager.Instance.InventoryUI.SpawnObject(itemType);

			HoldItemInHand();
		}

		/// <summary>
		/// Switch between items with mouse wheel or left/right bumpers.
		/// </summary>
		/// <param name="isNext"></param>
		public void NextItem(bool isNext)
		{
			// Scroll throught the list.
			CurrentActiveIndex = isNext ? CurrentActiveIndex + 1 : CurrentActiveIndex - 1;
			CurrentActiveIndex = CurrentActiveIndex.WithinCount(interactableObjectList.Count);

			if (CurrentActiveIndex >= interactableObjectList.Count - 1) return;

			// Do nothing if it's the same object.
			var newActiveObject = interactableObjectList[CurrentActiveIndex];
			if (activeObject == newActiveObject) return;

			// Set to new active gameobject.
			activeObject?.gameObject.SetActive(false);
			activeObject = newActiveObject;

			HoldItemInHand();
		}

		/// <summary>
		/// This will only apply to keyboard controls. Number 1~9 keys to select items from the list.
		/// </summary>
		/// <param name="index"></param>
		public void KeyboardButtonSelect(int index)
		{
			if (index < 0 || index > interactableObjectList.Count - 1) return;

			CurrentActiveIndex = index;

			// Do nothing if it's the same object.
			var newActiveObject = interactableObjectList[CurrentActiveIndex];
			if (activeObject == newActiveObject) return;

			// Set to new active gameobject.
			activeObject?.gameObject.SetActive(false);
			activeObject = newActiveObject;

			HoldItemInHand();
		}

		/// <summary>
		/// Put it near the player view.
		/// </summary>
		void HoldItemInHand()
		{
			Transform activeTrans = activeObject.transform;

			activeTrans.SetParent(FPSHoldItemInHandView);
			activeTrans.localPosition = initialPosition;
			activeTrans.localRotation = Quaternion.identity;
			activeTrans.localScale = initialScale;

			activeObject.gameObject.SetActive(true);
			FPS_ShowItem(true);
		}

		/// <summary>
		/// Animation of it moving to the hand/away from hand.
		/// </summary>
		/// <param name="toPosition"></param>
		void AnimateActiveItem(Vector3 toPosition)
		{
			Transform activeTrans = activeObject.transform;

			comeIntoViewCR?.StopCoroutine();
			comeIntoViewCR = CoroutineHelper.LerpFromTo(activeTrans, activeTrans.localPosition, toPosition, 0.3f);
		}

		void OnApplicationQuit()
		{
			inputControllerInfo.OnEnableEvent -= FPS_ShowItem;
		}
	}
}