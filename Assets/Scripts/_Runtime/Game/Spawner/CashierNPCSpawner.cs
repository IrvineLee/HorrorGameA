using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

using Personal.Manager;
using Personal.FSM.Cashier;
using Personal.Definition;
using Personal.GameState;

namespace Personal.Spawner
{
	public class CashierNPCSpawner : SpawnerBase
	{
		[SerializeField] TargetInfo targetInfo = null;
		[SerializeField] CashierInteractionDefinition cashierInteractionDefinition = null;

		protected override async UniTask Start()
		{
			await base.Start();

			StageManager.Instance.RegisterCashierNPCSpawner(this);
			cashierInteractionDefinition.Initalize();
		}

		/// <summary>
		/// Spawn cashier actor with interactions.
		/// </summary>
		public async void SpawnCashierActor()
		{
			int dayID = StageManager.Instance.DayIndex + 1;
			int interactionID = StageManager.Instance.CashierInteractionIndex + 1;

			var key = new MasterCashierNPC.DayInteraction(dayID, interactionID);
			var entity = MasterDataManager.Instance.CashierNPC.Get(key);

			// Spawn the actor.
			GameObject instance = await Spawn(entity.characterPath, targetInfo.SpawnAt.position);

			// Set the interaction.
			CashierInteraction cashierInteraction = cashierInteractionDefinition.GetInteraction(entity.interactionPath);
			CashierStateMachine instanceFSM = instance.GetComponentInChildren<CashierStateMachine>();
			instanceFSM.Initialize(targetInfo, cashierInteraction.OrderedStateList);
		}

		/// <summary>
		/// Handle the spawning of objects.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected override async UniTask<GameObject> Spawn(string path, Vector3 position = default)
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);
			await UniTask.WaitUntil(() => handle.Status != AsyncOperationStatus.None);

			return Addressables.InstantiateAsync(path, position, Quaternion.identity).Result;
		}
	}
}