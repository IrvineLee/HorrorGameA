using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class InteractionAssign : MonoBehaviour
	{
		[SerializeField] StateMachineBase interactionPrefab = null;

		public bool IsComplete { get; private set; }

		public List<StateBase> OrderedStateList { get; private set; } = new();
		public StateMachineBase SpawnedFSM { get; private set; } = null;

		Dictionary<int, InteractionAssignID> interactionAssignDictionary = new();

		void Start()
		{
			// Try to get the already existed stateMachine in scene.
			SpawnedFSM = GetComponentInChildren<StateMachineBase>();
			if (SpawnedFSM) Initialize();
		}

		public StateMachineBase SpawnInteraction()
		{
			if (SpawnedFSM) return null;

			SpawnedFSM = Instantiate(interactionPrefab, transform);
			Initialize();

			return SpawnedFSM;
		}

		public async UniTask BeginFSM(StateMachineBase initiatorStateMachine)
		{
			await SpawnedFSM.Begin(this, initiatorStateMachine);
		}

		public void SetToComplete() { IsComplete = true; }

		void Initialize()
		{
			// Get the states.
			foreach (Transform child in SpawnedFSM.transform)
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