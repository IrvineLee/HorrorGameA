using UnityEngine;
using System.Text;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.Definition;
using Personal.GameState;
using Personal.FSM;
using Personal.FSM.Character;

namespace Personal.Spawner
{
	public class CashierNPCSpawner : SpawnerBase
	{
		[SerializeField] CashierInteractionDefinition cashierInteractionDefinition = null;
		[SerializeField] TargetInfo targetInfo = null;

		StringBuilder sb = new StringBuilder();

		protected override void EarlyInitialize()
		{
			StageManager.Instance.RegisterCashierNPCSpawner(this);
			cashierInteractionDefinition.Initalize();
		}

		/// <summary>
		/// Spawn cashier actor with interactions.
		/// </summary>
		public async UniTask SpawnCashierActor()
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
			GameObject npcInstance = PoolManager.Instance.GetSpawnedObject(entity.characterPath);
			if (!npcInstance) npcInstance = await Spawn(entity.characterPath, targetInfo.SpawnTarget.position);

			// Set the interaction.
			var npcFSM = npcInstance.GetComponentInChildren<NPCStateMachine>();
			var interactionAssign = npcInstance.GetComponentInChildren<InteractionAssign>();

			var interactionPrefab = cashierInteractionDefinition.GetInteraction(sb.ToString());
			interactionAssign.SetInteractionPrefab(interactionPrefab);
			interactionAssign.SpawnInteraction(npcFSM);

			npcFSM.SetTargetInfo(targetInfo);
			interactionAssign.BeginFSM(npcFSM).Forget();

			Debug.Break();
		}
	}
}