using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Helper;
using Personal.Spawner;
using Cysharp.Threading.Tasks;
using Personal.Character.Player;

namespace Personal.Manager
{
	public class StageManager : MonoBehaviourSingleton<StageManager>
	{
		public Camera MainCamera { get; private set; }
		public PlayerController PlayerController { get; private set; }
		public CashierNPCSpawner CashierNPCSpawner { get; private set; }

		public int DayIndex { get; private set; }
		public int CashierInteractionIndex { get; private set; }

		async void Start()
		{
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);

			MainCamera = Camera.main;
			PlayerController = FindObjectOfType<PlayerController>();
		}

		public void RegisterCashierNPCSpawner(CashierNPCSpawner cashierNPCSpawner)
		{
			CashierNPCSpawner = cashierNPCSpawner;
		}

		public void NextDay()
		{
			DayIndex++;
			CashierInteractionIndex = 0;
		}
	}
}

