using UnityEngine;
using UnityEngine.AddressableAssets;

using Cysharp.Threading.Tasks;
using Personal.FSM.Character;
using Personal.Manager;
using Helper;

namespace Personal.FSM.Cashier
{
	public class PlayerCashierScanAndPackState : PlayerStandardState
	{
		OrderedStateMachine cashierStateMachine;
		GameObject spawnedObject;

		int currentCount, maxCount;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			cashierStateMachine = (OrderedStateMachine)stateMachine;

			// There will always be only 1 child within this transform.
			Transform itemSelectionParent = transform.GetChild(0);

			CashierItemSet cashierItemSet = itemSelectionParent.GetComponentInChildren<CashierItemSet>();
			Vector3 position = cashierStateMachine.TargetInfo.PlaceToPutItem.position;
			spawnedObject = await AddressableHelper.Spawn(cashierItemSet.CashierItemType.GetStringValue(), position);

			maxCount = itemSelectionParent.childCount;
			await UniTask.WaitUntil(() => currentCount == maxCount);
		}

		public override void HandleInteractable(RaycastHit hit)
		{
			if (!InputManager.Instance.FPSInputController.IsInteract) return;

			currentCount++;
			hit.transform.gameObject.SetActive(false);
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			Addressables.Release(spawnedObject);
			currentCount = 0;
		}
	}
}