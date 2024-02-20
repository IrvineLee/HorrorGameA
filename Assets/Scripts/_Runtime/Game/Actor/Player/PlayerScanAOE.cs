using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.InteractiveObject;

namespace Personal.Character.Player
{
	public class PlayerScanAOE : GameInitialize
	{
		async UniTaskVoid OnTriggerEnter(Collider other)
		{
			if (!isInitialized) await UniTask.WaitUntil(() => isInitialized);

			var interactableObject = other.gameObject.GetComponentInParent<InteractableObject>();
			if (!interactableObject) return;

			if (interactableObject.IsInteractable)
			{
				interactableObject.SetPointOfInterest(true);
			}
		}

		void OnTriggerExit(Collider other)
		{
			var interactableObject = other.gameObject.GetComponentInParent<InteractableObject>(true);
			interactableObject?.SetPointOfInterest(false);
		}
	}
}