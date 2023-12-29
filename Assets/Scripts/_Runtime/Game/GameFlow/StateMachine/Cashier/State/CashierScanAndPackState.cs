using UnityEngine;
using UnityEngine.AddressableAssets;

using Cysharp.Threading.Tasks;
using Personal.FSM.Character;
using Personal.Manager;
using Helper;
using System.Collections.Generic;

namespace Personal.FSM.Cashier
{
	public class CashierScanAndPackState : StateBase
	{
		[SerializeField] Transform itemSelectionParent = null;

		OrderedStateMachine orderedStateMachine;
		GameObject spawnedObject;

		List<Transform> childList = new();

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			orderedStateMachine = (OrderedStateMachine)stateMachine;

			// There will always be only 1 child within this transform.
			//Transform itemSelectionParent = transform.GetChild(0);

			CashierItemSet cashierItemSet = itemSelectionParent.GetComponentInChildren<CashierItemSet>(true);
			Vector3 position = orderedStateMachine.TargetInfo.PlaceToPutItem.position;
			spawnedObject = await AddressableHelper.Spawn(cashierItemSet.CashierItemType.GetStringValue(), position);

			StageManager.Instance.PlayerController.FSM.SwitchToState(typeof(PlayerCashierState)).Forget();

			childList.Clear();
			foreach (Transform child in spawnedObject.transform)
			{
				childList.Add(child);
			}

			await UniTask.WaitUntil(() => IsTakenAllItems(), cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		bool IsTakenAllItems()
		{
			for (int i = childList.Count - 1; i >= 0; i--)
			{
				Transform child = childList[i];
				if (!child.gameObject.activeSelf) childList.RemoveAt(i);
			}

			if (childList.Count > 0) return false;
			return true;
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			StageManager.Instance.PlayerController.FSM.SwitchToState(typeof(PlayerStandardState)).Forget();
			Addressables.Release(spawnedObject);
		}
	}
}