using UnityEngine;
using System.Text;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.FSM.Cashier;
using Personal.Definition;
using Personal.GameState;
using Helper;

namespace Personal.Spawner
{
	public class CashierNPCSpawner : SpawnerBase
	{
		[SerializeField] CashierInteractionDefinition cashierInteractionDefinition = null;
		[SerializeField] TargetInfo targetInfo = null;

		StringBuilder sb = new StringBuilder(30, 50);

		void Start()
		{
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
			GameObject instance = PoolManager.Instance.GetSpawnedActor(entity.characterPath);
			if (!instance) instance = await Spawn(entity.characterPath, targetInfo.SpawnAtFirst.position);

			// Set the interaction.
			CashierInteraction cashierInteraction = cashierInteractionDefinition.GetInteraction(sb.ToString());
			NPCCashierStateMachine instanceFSM = instance.GetComponentInChildren<NPCCashierStateMachine>();
			instanceFSM.Initialize(targetInfo, cashierInteraction.OrderedStateList);
		}
	}
}