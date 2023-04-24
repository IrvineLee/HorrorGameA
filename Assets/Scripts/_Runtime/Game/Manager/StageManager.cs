using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Helper;
using Personal.Definition;
using Personal.Spawner;
using Personal.FSM.Cashier;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class StageManager : MonoBehaviourSingleton<StageManager>
	{
		[SerializeField] CashierInteractionDefinition cashierInteractionDefinition = null;

		public Camera MainCamera { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }

		async void Start()
		{
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);

			MainCamera = Camera.main;
			cashierInteractionDefinition.Initalize();
		}

		public void RegisterCashierNPCSpawner(CashierNPCSpawner cashierNPCSpawner)
		{
			CashierNPCSpawner = cashierNPCSpawner;
		}

		public async void SpawnCashierActor()
		{
			var key = new MasterCashierNPC.DayInteraction(DayIndex + 1, CashierInteractionIndex + 1);
			var entity = MasterDataManager.Instance.CashierNPC.Get(key);

			GameObject instance = await CashierNPCSpawner.Spawn(entity.characterPath);
			CashierStateMachine instanceFSM = instance.GetComponentInChildren<CashierStateMachine>();

			CashierInteraction cashierInteraction = cashierInteractionDefinition.GetInteraction(entity.interactionPath);
			instanceFSM.SetAndPlayOrderedStateList(cashierInteraction.OrderedStateList);
		}

		public void NextDay()
		{
			DayIndex++;
			CashierInteractionIndex = 0;
		}
	}
}

