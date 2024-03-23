using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class InteractionAssign : MonoBehaviour
	{
		[SerializeField] Transform interactionPrefab = null;
		[SerializeField] bool isEndDestroyInteraction = true;

		// Whether the process is completed. Typically for puzzles etc..
		public bool IsProcessComplete { get; private set; }

		public List<StateBase> OrderedStateList { get; private set; } = new();
		public StateMachineBase FSM { get => fsm; }

		Transform spawnedPrefab;
		StateMachineBase fsm;

		Dictionary<int, InteractionAssignID> interactionAssignDictionary = new();

		void Awake()
		{
			// Try to get the already existed stateMachine in scene.
			fsm = GetComponentInChildren<StateMachineBase>();
			if (fsm)
			{
				spawnedPrefab = fsm.transform;
				Initialize();
			}
		}

		/// <summary>
		/// Put in the stateMachine when you're not using the prefab's stateMachine.
		/// </summary>
		/// <param name="stateMachine"></param>
		public void SpawnInteraction(StateMachineBase stateMachine = null)
		{
			if (fsm) return;

			spawnedPrefab = Instantiate(interactionPrefab, transform);
			fsm = spawnedPrefab.GetComponentInChildren<StateMachineBase>();

			if (stateMachine) fsm = stateMachine;
			Initialize();

			return;
		}

		public void DestroyInteraction()
		{
			if (!isEndDestroyInteraction) return;
			Destroy(fsm.gameObject);
		}

		public async UniTask BeginFSM(StateMachineBase initiatorStateMachine)
		{
			await fsm.Begin(this, initiatorStateMachine);
		}

		/// <summary>
		/// Typicaly used when actor is spawned without any interactionPrefab set.
		/// </summary>
		/// <param name="interactionPrefab"></param>
		public void SetInteractionPrefab(Transform interactionPrefab) { this.interactionPrefab = interactionPrefab; }

		/// <summary>
		/// Set the process complete. Typically for puzzles etc.. 
		/// </summary>
		public void SetProcessComplete() { IsProcessComplete = true; }

		void Initialize()
		{
			// Get the states.
			foreach (Transform child in spawnedPrefab)
			{
				OrderedStateList.Add(child.GetComponent<StateBase>());
			}

			// Get the id in the scene.
			var interactionAssignIDArray = GetComponentsInChildren<InteractionAssignID>();
			interactionAssignDictionary = interactionAssignIDArray.ToDictionary(i => i.ID);

			// Set the gameobjects to the prefab.
			foreach (var state in OrderedStateList)
			{
				var stateWithID = state.GetComponent<StateWithID>();
				if (!stateWithID) continue;

				var moveToTarget = GetTargetWithID(stateWithID.MoveToTargetID);
				var lookAtTarget = GetTargetWithID(stateWithID.LookAtTargetID);

				stateWithID.SetTarget(moveToTarget, lookAtTarget);
			}
		}

		Transform GetTargetWithID(int id)
		{
			if (!interactionAssignDictionary.TryGetValue(id, out InteractionAssignID interactionAssignID))
			{
				if (id != -1) Debug.Log("Couldn't get gameobject for " + id);
				return null;
			}
			return interactionAssignID.transform;
		}
	}
}