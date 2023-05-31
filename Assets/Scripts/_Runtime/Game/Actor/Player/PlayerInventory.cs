using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.Object;
using Personal.GameState;
using Personal.Manager;
using Personal.InputProcessing;
using Helper;
using static Personal.Definition.InputReaderDefinition;

namespace Personal.Character.Player
{
	public class PlayerInventory : GameInitialize
	{
		[SerializeField] Transform inventoryView = null;

		[SerializeField]
		[ReadOnly]
		InteractableObject activeObject = null;

		[SerializeField]
		[ReadOnly]
		List<InteractableObject> interactableObjectList = new();

		public InteractableObject ActiveObject { get => activeObject; }
		public List<InteractableObject> InteractableObjectList { get => interactableObjectList; }

		int currentActiveIndex;

		Vector3 initialPosition = new Vector3(0, -0.25f, 0);
		Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f);

		CoroutineRun comeIntoViewCR = new CoroutineRun();

		InputControllerInfo inputControllerInfo;

		protected async override UniTask Awake()
		{
			await base.Awake();

			inputControllerInfo = InputManager.Instance.GetInputControllerInfo(ActionMapType.Player);
			inputControllerInfo.OnEnableEvent += ShowItem;
		}

		/// <summary>
		/// Show or hide the active object.
		/// </summary>
		/// <param name="isFlag"></param>
		public void ShowItem(bool isFlag)
		{
			if (isFlag) AnimateActiveItem(Vector3.zero);
			else AnimateActiveItem(initialPosition);
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
			currentActiveIndex = interactableObjectList.Count - 1;

			SetToInventoryView();
		}

		/// <summary>
		/// Switch between items with mouse wheel or left/right bumpers.
		/// </summary>
		/// <param name="isNext"></param>
		public void NextItem(bool isNext)
		{
			// Scroll throught the list.
			currentActiveIndex = isNext ? currentActiveIndex + 1 : currentActiveIndex - 1;
			currentActiveIndex = currentActiveIndex.WithinCount(interactableObjectList.Count);

			// Do nothing if it's the same object.
			var newActiveObject = interactableObjectList[currentActiveIndex];
			if (activeObject == newActiveObject) return;

			// Set to new active gameobject.
			activeObject.gameObject.SetActive(false);
			activeObject = newActiveObject;

			SetToInventoryView();
		}

		/// <summary>
		/// This will only apply to keyboard controls. Number 1~9 keys to select items from the list.
		/// </summary>
		/// <param name="index"></param>
		public void KeyboardButtonSelect(int index)
		{
			if (index < 0 || index > interactableObjectList.Count - 1) return;

			currentActiveIndex = index;

			// Do nothing if it's the same object.
			var newActiveObject = interactableObjectList[currentActiveIndex];
			if (activeObject == newActiveObject) return;

			// Set to new active gameobject.
			activeObject.gameObject.SetActive(false);
			activeObject = newActiveObject;

			SetToInventoryView();
		}

		/// <summary>
		/// Put it near the player view.
		/// </summary>
		void SetToInventoryView()
		{
			Transform activeTrans = activeObject.transform;

			activeTrans.SetParent(inventoryView);
			activeTrans.localPosition = initialPosition;
			activeTrans.localRotation = Quaternion.identity;
			activeTrans.localScale = initialScale;

			activeObject.gameObject.SetActive(true);
			AnimateActiveItem(Vector3.zero);
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
			inputControllerInfo.OnEnableEvent -= ShowItem;
		}
	}
}