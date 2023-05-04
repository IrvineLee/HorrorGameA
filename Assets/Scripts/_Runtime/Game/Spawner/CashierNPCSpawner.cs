using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

using Personal.Manager;
using Personal.FSM.Cashier;
using Personal.Definition;
using Personal.GameState;
using Helper;
using System.Text;

namespace Personal.Spawner
{
	public class CashierNPCSpawner : SpawnerBase
	{
		[SerializeField] TargetInfo targetInfo = null;
		[SerializeField] CashierInteractionDefinition cashierInteractionDefinition = null;

		StringBuilder sb = new StringBuilder(30, 50);

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

			// Update the path to follow the key((##)dayID and (**)interactionID) from MasterCashierNPC data.
			string dayStr = StringHelper.AddSymbolInFront('0', 2, dayID.ToString());
			string interactionStr = StringHelper.AddSymbolInFront('0', 2, interactionID.ToString());

			sb.Length = 0;
			sb.Append(entity.interactionPath);
			sb.Replace("##", dayStr);
			sb.Replace("**", interactionStr);

			// Spawn the actor.
			GameObject instance = await Spawn(entity.characterPath, targetInfo.SpawnAt.position);

			// Set the interaction.
			CashierInteraction cashierInteraction = cashierInteractionDefinition.GetInteraction(sb.ToString());
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