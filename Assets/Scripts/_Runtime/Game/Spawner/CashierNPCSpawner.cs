using UnityEngine;
using System.Text;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Definition;
using Personal.GameState;
using Personal.FSM;
using Helper;

namespace Personal.Spawner
{
	public class CashierNPCSpawner : SpawnerBase
	{
		[SerializeField] CashierInteractionDefinition cashierInteractionDefinition = null;
		[SerializeField] TargetInfo targetInfo = null;

		StringBuilder sb = new StringBuilder();

		void Awake()
		{
			StageManager.Instance.RegisterCashierNPCSpawner(this);
			cashierInteractionDefinition.Initalize();
		}

		/// <summary>
		/// Spawn cashier actor with interactions.
		/// </summary>
		public async void SpawnCashierActor()
		{
			StageManager.Instance.NextInteraction();

			int dayID = StageManager.Instance.DayIndex + 1;
			int interactionID = StageManager.Instance.CashierInteractionIndex;

			var key = new MasterCashierNPC.DayInteraction(dayID, interactionID);
			var entity = MasterDataManager.Instance.CashierNPC.Get(key);

			// Update the path to follow the key((##)dayID and (**)interactionID) from MasterCashierNPC data.
			string dayStr = dayID.ToString().AddSymbolInFront('0', 2);
			string interactionStr = interactionID.ToString().AddSymbolInFront('0', 2);

			sb.Clear();
			sb.Append(entity.interactionPath);
			sb.Replace("##", dayStr);
			sb.Replace("**", interactionStr);

			// Spawn the actor.
			GameObject instance = PoolManager.Instance.GetSpawnedObject(entity.characterPath);
			if (!instance) instance = await Spawn(entity.characterPath, targetInfo.SpawnAtFirst.position);

			// Set the interaction.
			InteractionAssign interactionAssign = cashierInteractionDefinition.GetInteraction(sb.ToString());
			OrderedStateMachine instanceFSM = instance.GetComponentInChildren<OrderedStateMachine>();
			instanceFSM.Begin(instanceFSM, targetInfo, interactionAssign).Forget();
		}
	}
}