using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Item;
using Personal.Manager;
using Personal.Save;

namespace Personal.InteractiveObject
{
	public class InteractableUseActiveItem : InteractableObject, IDataPersistence
	{
		[SerializeField] protected ItemType itemTypeCompare = ItemType._None;
		[SerializeField] protected Transform placeAt = null;

		protected override UniTask HandleInteraction()
		{
			HandleUseActiveItem();
			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Check whether it's the correct item type before using it.
		/// </summary>
		async void HandleUseActiveItem()
		{
			var activeObject = StageManager.Instance.PlayerController.Inventory.ActiveObject;

			if (activeObject == null) return;
			if (itemTypeCompare != activeObject.ItemType) return;

			await SpawnItem();
			StageManager.Instance.PlayerController.Inventory.UseActiveItem();

			await base.HandleInteraction();
		}

		async UniTask SpawnItem()
		{
			interactableState = InteractableState.EndNonInteractable;
			var instance = await AddressableHelper.Spawn(itemTypeCompare.GetStringValue(), placeAt.position, placeAt.parent, true);

			var collider = instance.GetComponentInChildren<Collider>();
			if (collider) collider.enabled = false;
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			data.SceneObjectSavedData.PickupableDictionary.AddOrUpdateValue(guid, interactableState);
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			if (!data.SceneObjectSavedData.PickupableDictionary.TryGetValue(guid, out interactableState)) return;
			if (interactableState == InteractableState.EndNonInteractable) return;

			SpawnItem().Forget();
		}
	}
}

