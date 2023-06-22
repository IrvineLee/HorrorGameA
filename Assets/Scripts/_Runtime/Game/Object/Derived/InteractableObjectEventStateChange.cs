using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.InteractiveObject
{
	public class InteractableObjectEventStateChange : InteractableObject
	{
		[SerializeField] protected ActionMapType actionMapType = ActionMapType.Player;

		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;

		protected override void Initialize()
		{
			base.Initialize();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
		}

		protected override async UniTask HandleInteraction()
		{
			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();

			ifsmHandler?.OnBegin(null);
			InputManager.Instance.EnableActionMap(actionMapType);

			await orderedStateMachine.Begin(InitiatorStateMachine, null, interactionAssign);

			InputManager.Instance.SetToDefaultActionMap();
			ifsmHandler?.OnExit();
		}
	}
}

