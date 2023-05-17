using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM.Character;
using Helper;
using UnityEngine.AddressableAssets;

namespace Personal.FSM.Cashier
{
	public class CashierScanAndPackState : PlayerStandardState
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
			spawnedObject = await AddressableHelper.Spawn(cashierItemSet.CashierItemType.GetStringValue(), cashierStateMachine.TargetInfo.PlaceToPutItem.position);

			maxCount = itemSelectionParent.childCount;
			await UniTask.WaitUntil(() => currentCount == maxCount);

			return;
		}

		public override void OnHitInteractable(RaycastHit hit)
		{
			// TODO : Hacking, should use the new input system for this.
			if (Input.GetMouseButtonDown(0))
			{
				currentCount++;
				hit.transform.gameObject.SetActive(false);
			}
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			Addressables.Release(spawnedObject);
			currentCount = 0;
		}
	}
}