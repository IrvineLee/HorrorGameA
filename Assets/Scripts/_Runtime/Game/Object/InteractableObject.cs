using UnityEngine;

using Personal.GameState;
using Personal.FSM;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class InteractableObject : GameInitialize
	{
		public enum InteractType
		{
			Pickupable = 0,
			StateChange,
		}

		[SerializeField] InteractType interactType = InteractType.Pickupable;

		OrderedStateMachine orderedStateMachine;
		InteractionAssign interactionAssign;

		protected override async UniTask Awake()
		{
			await base.Awake();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
		}

		public void HandleInteraction()
		{
			if (interactType == InteractType.Pickupable)
			{

			}
			else if (interactType == InteractType.StateChange)
			{
				orderedStateMachine.Initialize(null, interactionAssign);
			}
			//// TODO: Item enter into inventory.
			//Debug.Log(hit.transform.name);
		}
	}
}

