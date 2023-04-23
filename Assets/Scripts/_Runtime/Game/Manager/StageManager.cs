﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Helper;
using Personal.Definition;
using Personal.Spawner;
using Personal.FSM.Cashier;

namespace Personal.Manager
{
	public class StageManager : MonoBehaviourSingleton<StageManager>
	{
		[SerializeField] List<CashierInteractionDefinition> cashierInteractionDefinitionList = new List<CashierInteractionDefinition>();

		public Camera MainCamera { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }

		IEnumerator Start()
		{
			yield return new WaitUntil(() => GameManager.Instance.IsLoadingOver);

			MainCamera = Camera.main;
		}

		public void RegisterCashierNPCSpawner(CashierNPCSpawner cashierNPCSpawner)
		{
			CashierNPCSpawner = cashierNPCSpawner;
		}

		public async void SpawnCashierActor()
		{
			var key = new MasterCashierNPC.DayInteraction(DayIndex + 1, CashierInteractionIndex + 1);
			string path = MasterDataManager.Instance.CashierNPC.Get(key).characterPath;

			GameObject instance = await CashierNPCSpawner.Spawn(path);
			CashierStateMachine instanceFSM = instance.GetComponentInChildren<CashierStateMachine>();

			CashierInteraction cashierInteraction = cashierInteractionDefinitionList[DayIndex].StateList[CashierInteractionIndex];
			instanceFSM.SetAndPlayOrderedStateList(cashierInteraction.OrderedStateList);
		}

		public void NextDay()
		{
			DayIndex++;
			CashierInteractionIndex = 0;
		}
	}
}

